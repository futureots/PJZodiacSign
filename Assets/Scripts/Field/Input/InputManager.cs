using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;



public class InputManager : Singleton<InputManager>
{
    //false일때 입력을 받고 true이면 입력을 받지 않음
    public static bool isInputStop;
    public Vector2 PointerPosition { get; private set; }

    GameInputActions _inputActions;

    // 입력 표시자
    public AreaVisualizer areaVisualizer;

    // 플레이어 컨트롤러
    public EntityController controller;

    // 표시 이펙트
    public GameObject entitySelecter;
    public GameObject tileSelecter;
    public GameObject skillSelecter;

    #region InputMode
    public enum Mode
    {
        Move,//이동 입력(드래그&드롭)
        Skill,//스킬 입력(클릭)
        None // 입력 X, 정보만 표시
    }
    public Mode currentMode;
    IModeInput curModeState;
    /// <summary>
    /// 모드 변경 및 입력 세팅(스킬 입력은 제외)
    /// </summary>
    public void SetInputMode(Mode mode)
    {
        if (isInputStop) return;
        curModeState?.RemoveMode();
        currentMode = mode;
        switch (mode)
        {
            case Mode.Move:
                curModeState = new MoveModeInput(_inputActions);
                break;
            case Mode.None:
                curModeState = new EmptyModeInput();
                break;
        }
        curModeState.SetMode();
    }
    public void SetInputMode() => SetInputMode(Mode.Move);
    public void SetInputMode(Skill skill)
    {
        curModeState?.RemoveMode();
        currentMode = Mode.Skill;
        curModeState = new SkillModeInput(_inputActions, skill);
        curModeState.SetMode();
    }
    #endregion

    #region ClickInfo
    /// <summary>
    /// 클릭 시 Ray로 부딪힌 기물의 정보 UI 표시하기
    /// </summary>
    private void HandleClick()
    {

        Ray ray = Camera.main.ScreenPointToRay(PointerPosition);
        // 부딪힌 기물, (타일) UI 표시 
        if (Physics.Raycast(ray, out var hit))
        {
            var entity = hit.collider.GetComponent<Entity>();
            if (entity != null)
            {
                Debug.Log($"Show {entity.name}'s Info");
                // UI 표시
                UIManager.Instance.entityInfoPanel.ShowPanel(entity);
            }
            // 다른 클릭 가능한 오브젝트 확인
        }
        else if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Hide");
            UIManager.Instance.entityInfoPanel.HidePanel();
        }

    }
    #endregion

    
    private void Awake()
    {
        _inputActions = new GameInputActions();
    }
    private void OnEnable() => _inputActions.Enable();
    private void OnDisable() => _inputActions.Disable();

    private void Start()
    {
        _inputActions.Gameplay.Point.performed += value => PointerPosition = value.ReadValue<Vector2>();
        _inputActions.Gameplay.Click.started += _ => HandleClick();
    }

    List<Tile> list = new List<Tile>();

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