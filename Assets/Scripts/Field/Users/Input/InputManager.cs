using PlayerInput;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{

    public Vector2 PointerPosition { get; private set; }

    public GameInputActions inputActions { get; private set; }

    public IInputState curModeState { get; private set; }

    public Agent agent;

    // 표시 오브젝트
    public GameObject entitySelecter;
    public GameObject tileSelecter;
    public GameObject skillSelecter;
    public TurnType curTurnType { get; private set; }
    
    public Action<IInputState> onModeChanged;

    public Action<string, LogType> onMessageActivated; 

    [SerializeField] private int maxEntityCount = 12;
    public int  MaxEntityCount => maxEntityCount;
    protected void Awake()
    {
        inputActions = new GameInputActions();
    }

    private void Start()
    {
        // 마우스 클릭 시작 이벤트 트리거
        inputActions.Gameplay.Click.started += StartClick;

        // 마우스 클릭 취소 시 이벤트 트리거
        inputActions.Gameplay.Click.canceled += CancelClick;

        // 마우스 이동 시 이벤트 트리거
        inputActions.Gameplay.Point.performed += MoveMouse;

    }

    public void Init(Agent agent)
    {
        this.agent = agent;
        agent.fieldController.OnTurnStarted += OnTurnChange;
    }

    #region InputPackaging

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


    /// <summary>
    /// 현재 마우스 위치가 UI 위에 있는지 확인
    /// </summary>
    /// <returns></returns>
    bool IsOnUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = PointerPosition };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        if (results.Count > 0) return true;
        return false;
    }
    
    /// <summary>
    /// 마우스 클릭 시작
    /// </summary>
    /// <param name="context"></param>
    void StartClick(InputAction.CallbackContext context)
    {
        if (IsOnUI()) return;

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
        PointerPosition = context.ReadValue<Vector2>();
        OnMouseMove?.Invoke(PointerPosition);
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    #endregion

    #region Turn
    
    void OnTurnChange(Turn curTurn, uint count)
    {
        curTurnType = curTurn.type;
        
        if (curTurn.agentID == agent.id)
        {
            SetInputMode();
        }
        else
        {
            ClearInputMode();
        }
    }

    /// <summary>
    /// 입력이 종료되면(턴 종료 X) 실행할 함수
    /// </summary>
    public void ClearInputMode()
    {
        curModeState?.RemoveMode();
        curModeState = null;
        onModeChanged?.Invoke(curModeState);
    }

    public void SetInputMode()
    {
        curModeState?.RemoveMode();
        switch (curTurnType)
        {
            case TurnType.ACTION:
                curModeState = new MoveModeInput(this);
                break;
            case TurnType.REPAIR:
                curModeState = new RepairModeInput(this);
                break;
            case TurnType.ATTACK:
                curModeState = new AttackModeInput(this);
                break;
            default:
                curModeState = new EmptyModeInput();
                break;
        }
        onModeChanged?.Invoke(curModeState);
        curModeState.SetMode();
    }

    public void SetInputMode(SkillComponent skill)
    {
        curModeState?.RemoveMode();
        curModeState = new SkillModeInput(this, skill);
        onModeChanged?.Invoke(curModeState);
        curModeState.SetMode();
    }
    

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
