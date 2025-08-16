using DG.Tweening;
using PlayerInput;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InputManager : Agent
{
    public Vector2 PointerPosition { get; private set; }

    public GameInputActions inputActions { get; private set; }

    public Mode currentMode;
    IModeInput curModeState;

    // 입력 표시자
    public AreaVisualizer areaVisualizer;

    // 표시 이펙트
    public GameObject entitySelecter;
    public GameObject tileSelecter;
    public GameObject skillSelecter;

    // UI 패널
    [Header("UI Element")]
    public Button turnEndButton;
    public UIContainer UI;
    public GameObject cam;


    protected new void Awake()
    {
        base.Awake();
        inputActions = new GameInputActions();
    }

    private void Start()
    {
        

        // 오브젝트 클릭 시 오브젝트 이벤트 트리거
        inputActions.Gameplay.Click.started += StartClick;

        // 마우스 드롭 시 드롭 이벤트 트리거
        inputActions.Gameplay.Click.canceled += CancelClick;

        inputActions.Gameplay.Point.performed += MoveMouse;

        OnObjectClicked.AddListener(HandleClick);
    }
    #region InputPackaging

    /// <summary>
    /// 마우스 클릭 시작
    /// </summary>
    /// <param name="context"></param>
    void StartClick(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(PointerPosition);
        // 부딪힌 기물, (타일) UI 표시 
        if (Physics.Raycast(ray, out var hit))
        {
            var other = hit.collider.gameObject;
            OnObjectClicked?.Invoke(other);
        }
        else OnObjectClicked?.Invoke(null);
    }
    /// <summary>
    /// 마우스 클릭 떼기
    /// </summary>
    /// <param name="context"></param>
    void CancelClick(InputAction.CallbackContext context)
    {
        OnMouseUp?.Invoke();
    }
    /// <summary>
    /// 마우스 이동
    /// </summary>
    /// <param name="context"></param>
    void MoveMouse(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        PointerPosition = context.ReadValue<Vector2>();
        OnMouseMove?.Invoke(PointerPosition);
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    #endregion

    #region Phase
    public override void SetMode(Mode mode, Action call)
    {
        SetInputMode(mode);
        turnEndButton.onClick.AddListener(() =>
        {
            turnEndButton.onClick.RemoveAllListeners();
            call?.Invoke();
        });
    }
    public override void SetRepairPhase(int level)
    {
        base.SetRepairPhase(level);
        cam.transform.DOLocalMove(new Vector3(0, 0, -15),1f);
        UI.shop.SetShop(level);

        controller.OnCommandCreated += ExecuteCommand;
    }

    public override void EndRepairPhase()
    {
        controller.UpdateEntities();
        // 기물 데이터는 정비 턴 종료 시 업데이트
        var (field, hand) = controller.GetFieldData();
        data.fieldEntities = field;
        data.handEntities = hand;
        cam.transform.DOLocalMove(Vector3.zero, 1f);

        controller.OnCommandCreated -= ExecuteCommand;

        base.EndRepairPhase();
    }
    void ExecuteCommand(Command command)
    {
        Debug.Log("Command Execute");
        command?.Execute();
        SetInputMode(Mode.Repair);
    }

    public override void SetBattlePhase()
    {
        controller.OnCommandCreated += SetMoveMode;
    }
    public override void EndBattlePhase()
    {
        controller.OnCommandCreated -= SetMoveMode;
    }
    void SetMoveMode(Command cmd)
    {
        SetInputMode(Mode.Move);
    }


    #endregion

    #region InputMode

    /// <summary>
    /// 모드 변경 및 입력 세팅(스킬 입력은 제외)
    /// </summary>
    public void SetInputMode(Mode mode)
    {
        curModeState?.RemoveMode();
        currentMode = mode;
        switch (mode)
        {
            case Mode.Move:
                curModeState = new MoveModeInput(this);
                break;
            case Mode.Repair:
                curModeState = new RepairModeInput(this);
                break;
            case Mode.None:
                curModeState = new EmptyModeInput();
                break;
        }
        curModeState.SetMode();
    }
    public void SetInputMode(IActive active)
    {
        curModeState?.RemoveMode();
        currentMode = Mode.Skill;
        curModeState = new SkillModeInput(this, active);
        curModeState.SetMode();
    }
    #endregion

    #region ClickInfo

    /// <summary>
    /// 오브젝트가 기물이면 기물 정보 표시, 아니면 정보 패널 제거
    /// </summary>
    void HandleClick(GameObject obj)
    {
        if (obj == null)
        {
            UI.entityInfo.HidePanel();
            return;
        }
        var entity = obj.GetComponent<Entity>();
        if (entity != null)
        {
            UI.entityInfo.ShowPanel(entity);
        }
        else
        {
            UI.entityInfo.HidePanel();
        }

    }

    /// <summary>
    /// 오브젝트 클릭 시 트리거
    /// </summary>
    public UnityEvent<GameObject> OnObjectClicked;
    /// <summary>
    /// 마우스 드롭 시
    /// </summary>
    public UnityEvent OnMouseUp;
    /// <summary>
    /// 마우스 움직일 때마다 트리거
    /// </summary>
    public UnityEvent<Vector2> OnMouseMove;


    #endregion
    /// <summary>
    /// 벡터값에서 가장 가까운 타일 반환
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tiles"></param>
    /// <returns></returns>
    public Tile GetClosestTile(Vector3 pos, List<Tile> tiles)
    {
        float minDistance = 0;
        Tile closestTile = null;
        foreach (Tile tile in tiles)
        {
            var distance = (tile.transform.position - pos).magnitude;
            if (closestTile == null || minDistance > distance)
            {
                minDistance = distance;
                closestTile = tile;
            }
        }
        return closestTile;
    }
}
