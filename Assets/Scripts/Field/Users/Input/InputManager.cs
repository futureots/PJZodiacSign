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
    IPhaseManageService phaseService;

    public Vector2 PointerPosition { get; private set; }

    public GameInputActions inputActions { get; private set; }

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

        // GameManager.onNextLevel += AddCredit;
    }

    public void Init(IPhaseManageService phaseManageService)
    {
        phaseService = phaseManageService;
    }

    private void OnDestroy()
    {
        // GameManager.onNextLevel -= AddCredit;
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
        // if (EventSystem.current.IsPointerOverGameObject()) return;

        PointerPosition = context.ReadValue<Vector2>();
        OnMouseMove?.Invoke(PointerPosition);
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    #endregion

    #region Phase

    public override void SetRepairPhase(int level, Action call)
    {
        controller.SetResourceField(data.handEntities);
        controller.SetMainField(data.fieldEntities);

        cam.transform.DOLocalMove(new Vector3(0, -10, -15),1f);
        

        controller.onCommandCreated += ExecuteCommand;

        SetInputMode(PhaseType.Repair);
        turnEndButton.interactable = true;
        turnEndButton.onClick.AddListener(() =>
        {
            turnEndButton.onClick.RemoveAllListeners();
            call?.Invoke();
        });
    }

    public override void EndRepairPhase()
    {
        SetInputMode(PhaseType.None);
        controller.UpdateEntities();
        // 기물 데이터는 저장 시 저장소 업데이트
        var (field, hand) = controller.GetFieldData();
        data.fieldEntities = field;
        data.handEntities = hand;
        cam.transform.DOLocalMove(Vector3.up * -10, 1f);

        controller.onCommandCreated -= ExecuteCommand;

        base.EndRepairPhase();
    }
    void ExecuteCommand(Command command)
    {
        Debug.Log("Command Execute");
        command?.Execute();
        SetInputMode(PhaseType.Repair);
    }

    public override void SetActionTurn(Action call)
    {
        SetInputMode(PhaseType.Battle);
        Action<Command> bind = (x) =>
        {
            SetInputMode(PhaseType.Battle);
            turnEndButton.interactable = true;
        };
        controller.onCommandCreated += bind;
        turnEndButton.onClick.AddListener(() =>
        {
            SetInputMode(PhaseType.None);
            controller.onCommandCreated -= bind;
            turnEndButton.onClick.RemoveAllListeners();
            call?.Invoke();
            turnEndButton.interactable = false;
        });
        turnEndButton.interactable = false;
    }

    void AddCredit(int level)
    {
        // 이자 및 고정값 추가(나중에 기물 가격과 비교해서 밸런싱)
        Credit += Mathf.Min((int)(Credit * 0.1f), 50);
        Credit += 50;
    }
    #endregion

    #region InputMode

    /// <summary>
    /// 입력 모드 설정(이동 입력, 스킬 입력)
    /// </summary>
    public void SetInputMode(PhaseType mode)
    {
        curModeState?.RemoveMode();
        switch (mode)
        {
            case PhaseType.Battle:
                curModeState = new MoveModeInput(this);
                break;
            case PhaseType.Repair:
                curModeState = new RepairModeInput(this);
                break;
            case PhaseType.None:
                curModeState = new EmptyModeInput();
                break;
        }
        curModeState.SetMode();
    }
    
    public void SetInputMode(IActive active)
    {

        curModeState?.RemoveMode();

        curModeState = new SkillModeInput(this, active, phaseService);
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
