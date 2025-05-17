using System;
using UnityEngine;
using System.Reflection;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class InputManager : Singleton<InputManager>
{
    public Vector2 PointerPosition { get; private set; }

    GameInputActions _inputActions;

    public enum Mode
    {
        Move,//이동 입력(드래그&드롭)
        Skill,//스킬 입력(클릭)
        None // 입력 X, 정보만 표시
    }
    Mode _currentMode;
    public Mode currentMode {
        get
        {
            return _currentMode;
        }
        set
        {
            _currentMode = value;
            SetInputMode();
        }
    }
    /// <summary>
    /// 모드에 맞는 액션 추가
    /// </summary>
    public void SetInputMode()
    {
        Debug.Log("SetInputMode");
        _inputActions.Gameplay.Click.Reset();
        _inputActions.Gameplay.Click.performed += value => HandleClick();
        switch (currentMode)
        {
            case Mode.Move:
                SetSkill(null);
                SetDragInput();
                break;
            case Mode.Skill:
                break;
            case Mode.None:
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        _inputActions = new GameInputActions();
    }
    private void OnEnable() => _inputActions.Enable();
    private void OnDisable() => _inputActions.Disable();

    private void Start()
    {
        //기물 선택 정보 표시
        _inputActions.Gameplay.Point.performed += value => PointerPosition = value.ReadValue<Vector2>();
    }
    

    List<Tile> list = new List<Tile>();

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
                //UI 표시
                UIManager.Instance.entityInfoPanel.ShowPanel(entity);
            }
        }
    }
    #endregion

    #region Drag
    /// <summary>
    /// 마우스 드래그 드롭 기능 추가
    /// </summary>
    public void SetDragInput()
    {
        //선택한 엔티티 저장
        Entity _selectedEntity = null;
        GameObject targetSelecter = null;
        GameObject targetTileSelecter = null;
        Action<InputAction.CallbackContext> bindAction = null;
        _inputActions.Gameplay.Click.started += _ =>
        {
            
            Debug.Log("Started");
            Ray ray = Camera.main.ScreenPointToRay(PointerPosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var entity = hit.collider.GetComponent<Entity>();
                if (entity != null)
                {
                    // 적인지 아닌지 구분
                    if (!entity.CompareTag("Player")) return;
                    _selectedEntity = entity;
                    //값이 변경될 때마다 선택한 엔티티의 위치 이동
                    bindAction = value =>
                    {
                        Ray ray2 = Camera.main.ScreenPointToRay(value.ReadValue<Vector2>());
                        DragEntity(_selectedEntity, ray2,targetTileSelecter);
                    };
                    // 기물 이동범위 표시
                    targetSelecter = Instantiate(entitySelecter, entity.transform.position + Vector3.up * 0.1f, Quaternion.identity);
                    targetTileSelecter = Instantiate(tileSelecter, entity.transform.position, Quaternion.identity);
                    areaVisualizer.ShowMoveArea(entity.GetMoveArea());

                    _inputActions.Gameplay.Point.performed += bindAction;
                }
            }
        };
        _inputActions.Gameplay.Click.canceled += _ =>
        {
            // 엔티티 클리어
            if (_selectedEntity != null)
            {
                var tile = GetClosestTile(_selectedEntity.transform.position, _selectedEntity.GetMoveArea());
                controller.CreateCommand(_selectedEntity, tile, targetSelecter, targetTileSelecter);
                _selectedEntity.transform.position = _selectedEntity.curTile.transform.position;
                areaVisualizer.RemoveAttackArea(list);
                areaVisualizer.RemoveMoveArea(_selectedEntity.GetMoveArea());
                _selectedEntity = null;
            }
            _inputActions.Gameplay.Point.performed -= bindAction;
        };
    }
    /// <summary>
    /// 선택한 유닛의 위치 조정하기
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="ray"></param>
    void DragEntity(Entity entity, Ray ray,GameObject selecter)
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0, 10, 0));
        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            Vector3 pos = ray.GetPoint(rayDistance);
            entity.transform.position = pos;
        }
        // 공격 범위 표시
        var closeTile = GetClosestTile(entity.transform.position, entity.GetMoveArea());
        selecter.transform.position = closeTile.transform.position + Vector3.up * 0.1f;
        areaVisualizer.RemoveAttackArea(list);
        list = entity.GetAttackArea(closeTile);
        areaVisualizer.ShowAttackArea(list);
    }
    #endregion

    #region Old
    public EntityController controller;

    //false일때 입력을 받고 true이면 입력을 받지 않음
    public bool isInputStop;

    public AreaVisualizer areaVisualizer;

    public static event Action<GameObject> OnObjectMouseDown;
    public static event Action OnObjectMouseUp;

    public GameObject entitySelecter;
    public GameObject tileSelecter;
    public GameObject skillSelecter;

    public void OnGameObjectDown(GameObject selectObj)
    {
        if (isInputStop) return;
        OnObjectMouseDown?.Invoke(selectObj);
    }


    public void OnGameObjectUp()
    {
        if (isInputStop) return;
        OnObjectMouseUp?.Invoke();
    }
    #region MoveCommand관련

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
    #endregion
    #region SkillCommand 관련
    public ISkill selectedSkill { get; private set; }
    public void SetSkill(ISkill skill = null)
    {
        selectedSkill = skill;
    }
    public void AllocateSkillCommand(ISkill skill)
    {
        SetSkill(skill);
        OnObjectMouseDown = null;
        OnObjectMouseUp = null;
        StartCoroutine(SkillProcessCoroutine(skill));
    }
    /// <summary>
    /// skill에 필요한 입력을 받는 코루틴
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    IEnumerator SkillProcessCoroutine(ISkill skill)
    {
        Type type = skill.GetType();
        FieldInfo[] fieldInfo = type.GetFields();
        Debug.Log(fieldInfo.Length);
        List<GameObject> selecters = new();
        foreach (var field in fieldInfo)
        {
            var attr = (SkillTargetAttribute)field.GetCustomAttribute(typeof(SkillTargetAttribute));
            if (attr != null)
            {
                Debug.Log(attr.text);
                var fieldType = field.FieldType;

                GameObject selecter = Instantiate(skillSelecter);
                selecters.Add(selecter);
                selecter.SetActive(false);

                Action<GameObject> bindAction = (x) => SetFieldValue(skill, field, x);

                bindAction += (x) =>
                {
                    selecter.transform.position = x.transform.position + Vector3.up * 0.1f;
                };

                OnObjectMouseDown = bindAction;

                do
                {
                    yield return new WaitUntil(() => field.GetValue(skill) != null || skill != selectedSkill);
                } while (!skill.IsValidInput(field));

                selecter.SetActive(true);

                OnObjectMouseDown = null;
                if (skill != selectedSkill)
                {
                    Debug.Log("break");
                    yield break;
                }
                //InputManager.CleanAction(fieldType);
                Debug.Log($"field name : {field.Name} , field value : {field.GetValue(skill)}");
            }


        }
        Debug.Log("skill all allocated");
        controller.CreateCommand(skill, selecters.ToArray());
        SetSkill(null);
        //스킬 입력 완료
        //yield return new WaitForSeconds(1);
        //AllocateMoveCommand();
    }
    void SetFieldValue(ISkill skill, FieldInfo field, GameObject obj)
    {
        var component = obj.GetComponent(field.FieldType);
        field.SetValue(skill, component);
    }
    #endregion


    /*
    void ShowEntitySelecter()
    {
        if (!entitySelecter.activeSelf)
        {
            entitySelecter.SetActive(true);
            entitySelecter.transform.localScale = Vector3.one * controller.selectedEntity.transform.lossyScale.y;
        }
        entitySelecter.transform.position = new Vector3(controller.selectedEntity.transform.position.x, 0.1f, controller.selectedEntity.transform.position.z);

    }
    void ShowCellSelecter()
    {
        if (!tileSelecter.activeSelf)
        {
            tileSelecter.SetActive(true);
            tileSelecter.transform.localScale = Vector3.one * controller.selectedTile.transform.lossyScale.y * 4;
        }
        tileSelecter.transform.position = new Vector3(controller.selectedTile.transform.position.x, 0.1f, controller.selectedTile.transform.position.z);
    }
    */
    #endregion
}