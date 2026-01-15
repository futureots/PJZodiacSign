using PlayerInput;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;



public class InputManager : MonoBehaviour
{
    public static Action<InputManager> OnInitialized;

    public Vector2 PointerPosition { get; private set; }

    public GameInputActions inputActions { get; private set; }

    public IInputState curModeState { get; private set; }

    public Agent agent;

    // 표시 오브젝트
    public GameObject entitySelecter;
    public GameObject tileSelecter;
    public GameObject skillSelecter;

    public TurnType curTurnType { get; private set; }
    public Action<IInputState> OnModeChanged;

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

        agent.OnInitialized += () => Init(agent);
    }

    public void Init(Agent agent)
    {
        this.agent = agent;
        Agent.LocalPlayer = agent;
        EditorLogger.Print(agent.fieldController);
        agent.fieldController.onTurnStarted += OnTurnChange;
        OnInitialized?.Invoke(this);
        EditorLogger.Print("CallInputManagerInit");
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
    /// 마우스 클릭 시작
    /// </summary>
    /// <param name="context"></param>
    void StartClick(InputAction.CallbackContext context)
    {
        //if (EventSystem.current.IsPointerOverGameObject()) return;

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
        //if (EventSystem.current.IsPointerOverGameObject()) return;

        PointerPosition = context.ReadValue<Vector2>();
        OnMouseMove?.Invoke(PointerPosition);
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    #endregion

    #region Turn
    
    public void OnTurnChange(Turn curTurn)
    {
        curTurnType = curTurn.type;
        
        if (curTurn.agentID == agent.id)
        {
            EditorLogger.Print("Player" + curTurnType.ToString());
            if(curTurn.type == TurnType.ATTACK)
            {
                AttackInput();
            }
            else
            {
                SetInputMode();
            }
                
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
        OnModeChanged?.Invoke(curModeState);
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
            default:
                curModeState = new EmptyModeInput();
                break;
        }
        OnModeChanged?.Invoke(curModeState);
        curModeState.SetMode();
    }

    public void SetInputMode(SkillComponent skill)
    {
        curModeState?.RemoveMode();
        curModeState = new SkillModeInput(this, skill);
        OnModeChanged?.Invoke(curModeState);
        curModeState.SetMode();
    }

    public void AttackInput()
    {
        var entities = StageManager.Instance.field.GetEntities(agent.teamNum);
        foreach (var entity in entities)
        {
            agent.CreateAttackCommand(entity);
        }
        agent.CreateEndCommand();
        agent.SendCommand();
    }

    #endregion


    /// <summary>
    /// 오브젝트가 기물이면 기물 정보 표시, 아니면 정보 패널 숨김
    /// </summary>
    //void HandleClick(GameObject obj)
    //{
    //    if (obj == null)
    //    {
    //        UI.entityInfo.HidePanel();
    //        return;
    //    }
    //    var entity = obj.GetComponent<Entity>();
    //    if (entity != null)
    //    {
    //        UI.entityInfo.ShowPanel(entity);
    //    }
    //    else
    //    {
    //        UI.entityInfo.HidePanel();
    //    }

    //}

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
