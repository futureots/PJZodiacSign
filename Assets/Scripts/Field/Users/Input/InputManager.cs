using PlayerInput;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;


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
    //public GameObject cam;



    protected new void Awake()
    {
        base.Awake();
        inputActions = new GameInputActions();

    }
    private void Start()
    {
        //inputActions.Gameplay.Click.started += value => HandleClick(PointerPosition);

        // 오브젝트 클릭 시 오브젝트 이벤트 트리거
        inputActions.Gameplay.Click.started += value =>
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
        };

        // 마우스 드롭 시 드롭 이벤트 트리거
        inputActions.Gameplay.Click.canceled += value => OnMouseUp?.Invoke();

        inputActions.Gameplay.Point.performed += value =>
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            PointerPosition = value.ReadValue<Vector2>();
            OnMouseMove?.Invoke(PointerPosition);
        };

        OnObjectClicked.AddListener(HandleClick);
        
    }


    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

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

    public override void SetRepairField(int level)
    {
        base.SetRepairField(level);
        UI.shop.SetShop(level);
    }

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
        currentMode = Mode.Active;
        curModeState = new SkillModeInput(this, active);
        curModeState.SetMode();
    }
    public override void SetMode(Mode mode, Action call)
    {
        SetInputMode(mode);
        turnEndButton.onClick.AddListener(() =>
        {
            turnEndButton.onClick.RemoveAllListeners();
            call?.Invoke();
        });
    }

    public override void EndRepair()
    {
        controller.UpdateEntities();
        // 기물 데이터는 정비 턴 종료 시 업데이트
        var (field, hand) = controller.GetFieldData();
        data.fieldEntities = field;
        data.handEntities = hand;
        base.EndRepair();
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
}
