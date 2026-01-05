using PlayerInput;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;



public class InputManager : MonoBehaviour
{

    public Vector2 PointerPosition { get; private set; }

    public GameInputActions inputActions { get; private set; }

    public IModeInput curModeState { get; private set; }

    public Agent agent;

    // 표시 오브젝트
    public GameObject entitySelecter;
    public GameObject tileSelecter;
    public GameObject skillSelecter;
    public List<GameObject> visualizeObjects;

    public TurnType curTurnType { get; private set; }

    protected void Awake()
    {
        inputActions = new GameInputActions();
        visualizeObjects = new List<GameObject>();
    }

    private void Start()
    {
        // 마우스 클릭 시작 이벤트 트리거
        inputActions.Gameplay.Click.started += StartClick;

        // 마우스 클릭 취소 시 이벤트 트리거
        inputActions.Gameplay.Click.canceled += CancelClick;

        // 마우스 이동 시 이벤트 트리거
        inputActions.Gameplay.Point.performed += MoveMouse;

        if (TryGetComponent<Agent>(out var agent))
        {
            Init(agent);
        }
    }

    public void Init(Agent agent)
    {
        this.agent = agent;
        Debug.Log(agent.fieldController);
        agent.fieldController.onTurnStarted += OnTurnChanged;
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
    
    public void OnTurnChanged(Turn curTurn)
    {
        curTurnType = curTurn.type;
        
        if (curTurn.agentID == agent.id)
        {
            Debug.Log("Player" + curTurnType.ToString());
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
    }

    public void SetInputMode()
    {
        foreach (var obj in visualizeObjects)
        {
            Destroy(obj);
        }
        visualizeObjects.Clear();
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
        curModeState.SetMode();
    }

    public void SetInputMode(SkillComponent skill)
    {
        curModeState?.RemoveMode();
        curModeState = new SkillModeInput(this, skill);
        curModeState.SetMode();
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
