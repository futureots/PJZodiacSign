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

    // 입력 표시기
    public AreaVisualizer areaVisualizer;

    // 표시 오브젝트
    public GameObject entitySelecter;
    public GameObject tileSelecter;
    public GameObject skillSelecter;

    // UI 요소
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
        // 마우스 클릭 시작 이벤트 트리거
        inputActions.Gameplay.Click.started += StartClick;

        // 마우스 클릭 취소 시 이벤트 트리거
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
        // 레이캐스트 기물, (타일) UI 표시 
        if (Physics.Raycast(ray, out var hit))
        {
            var other = hit.collider.gameObject;
            OnObjectClicked?.Invoke(other);
        }
        else OnObjectClicked?.Invoke(null);
    }
    /// <summary>
    /// 마우스 클릭 취소
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

    public override void SetRepairPhase(int level, Action call)
    {
        controller.SetInstantField(data.handEntities);
        controller.SetMainField(data.fieldEntities);

        cam.transform.DOLocalMove(new Vector3(0, 0, -15),1f);
        

        controller.onCommandCreated += ExecuteCommand;

        SetInputMode(Mode.Repair);
        turnEndButton.onClick.AddListener(() =>
        {
            turnEndButton.onClick.RemoveAllListeners();
            call?.Invoke();
        });
    }

    public override void EndRepairPhase()
    {
        SetInputMode(Mode.None);
        controller.UpdateEntities();
        // 기물 데이터는 저장 시 저장소 업데이트
        var (field, hand) = controller.GetFieldData();
        data.fieldEntities = field;
        data.handEntities = hand;
        cam.transform.DOLocalMove(Vector3.zero, 1f);

        controller.onCommandCreated -= ExecuteCommand;

        base.EndRepairPhase();
    }
    void ExecuteCommand(Command command)
    {
        Debug.Log("Command Execute");
        command?.Execute();
        SetInputMode(Mode.Repair);
    }

    public override void SetActionTurn(Action call)
    {
        SetInputMode(Mode.Move);
        Action<Command> bind = (x) =>
        {
            SetInputMode(Mode.Move);
            turnEndButton.interactable = true;
        };
        controller.onCommandCreated += bind;
        turnEndButton.onClick.AddListener(() =>
        {
            SetInputMode(Mode.None);
            controller.onCommandCreated -= bind;
            turnEndButton.onClick.RemoveAllListeners();
            call?.Invoke();
        });
        turnEndButton.interactable = false;
    }

    #endregion

    #region InputMode

    /// <summary>
    /// 입력 모드 설정(이동 입력, 스킬 입력)
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

        Mode skillEndMode = Mode.None;
        if (PhaseManager.curPhase == PhaseType.Repair) skillEndMode = Mode.Repair;
        else if (PhaseManager.curPhase == PhaseType.Battle) skillEndMode = Mode.Move;

        curModeState = new SkillModeInput(this, active, skillEndMode);
        currentMode = Mode.Skill;
        curModeState.SetMode();
    }
    #endregion

    #region ClickInfo

    /// <summary>
    /// 오브젝트가 기물이면 기물 정보 표시, 아니면 정보 패널 숨김
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
    /// 오브젝트 클릭 시 이벤트
    /// </summary>
    public UnityEvent<GameObject> OnObjectClicked;
    /// <summary>
    /// 마우스 버튼 업
    /// </summary>
    public UnityEvent OnMouseUp;
    /// <summary>
    /// 마우스 이동 시 이벤트
    /// </summary>
    public UnityEvent<Vector2> OnMouseMove;


    #endregion
    /// <summary>
    /// 가장 가까운 타일 반환
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
