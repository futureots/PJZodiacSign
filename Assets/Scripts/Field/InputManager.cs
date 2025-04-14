using System;
using UnityEngine;
using Battle;
using System.Reflection;
using Unity.VisualScripting;
using System.Collections;

public class InputManager : Singleton<InputManager>
{
    public EntityController controller;

    //false일때 입력을 받고 true이면 입력을 받지 않음
    public bool isInputStop;
    public Battle.Entity selectedEntity;
    public ISkill selectedSkill { get; private set; }
    public void SetSkill(ISkill skill = null)
    {
        selectedSkill = skill;
    }


    public static event Action<GameObject> OnObjectMouseDown;
    public static event Action OnObjectMouseUp;
    #region inputMode
    public enum InputMode
    {
        //명령 없음(행동 X)
        None = -1,
        //초기 기물 세팅용
        Set = 0,
        //기물 이동(기물 범위 내 타일만 선택가능)
        Move = 1
    }
    public InputMode inputMode;
    #endregion

    public GameObject entitySelecter;
    public GameObject tileSelecter;

    private void Start()
    {
        entitySelecter = Instantiate(entitySelecter, transform);
        entitySelecter.SetActive(false);
        tileSelecter = Instantiate(tileSelecter, transform);
        tileSelecter.SetActive(false);
    }
    private void Update()
    {
        if (selectedEntity != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, 10, 0));
            float rayDistance;
            if (plane.Raycast(ray, out rayDistance))
            {
                Vector3 pos = ray.GetPoint(rayDistance);
                selectedEntity.transform.position = pos;
            }
        }
    }

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
    public void AllocateMoveCommand()
    {
        SetSkill(null);
        OnObjectMouseDown = SelectEntity;
        OnObjectMouseUp = ReleaseEntity;
    }
    /// <summary>
    /// Entity를 선택하는 함수
    /// </summary>
    /// <param name="selectObj"></param>
    void SelectEntity(GameObject selectObj)
    {
        var entity = selectObj.GetComponent<Battle.Entity>();
        if (entity == null) return;

        selectedEntity = entity;
        GameManager.Instance.field.AddFieldColor(0, entity.GetMoveArea().ToArray());
    }
    /// <summary>
    /// 선택한 Entity 제거 및 명령 전달
    /// </summary>
    void ReleaseEntity()
    {
        if (selectedEntity != null)
        {
            var tile = controller.GetClosestTile(selectedEntity.transform.position, GameManager.Instance.field);
            controller.CreateCommand(selectedEntity, tile);
            selectedEntity.transform.position = selectedEntity.curTile.transform.position;

            selectedEntity = null;
        }
    }

    #endregion
    #region SkillCommand 관련
    public void AllocateSkillCommand(ISkill skill)
    {
        SetSkill(skill);
        OnObjectMouseDown = null;
        OnObjectMouseUp = null;
        StartCoroutine(SkillProcessCoroutine(skill));
    }
    
    IEnumerator SkillProcessCoroutine(ISkill skill)
    {
        Type type = skill.GetType();
        FieldInfo[] fieldInfo = type.GetFields();
        Debug.Log(fieldInfo.Length);

        foreach (var field in fieldInfo)
        {
            var attr = (SkillTargetAttribute)field.GetCustomAttribute(typeof(SkillTargetAttribute));
            if (attr != null)
            {
                Debug.Log(attr.text);
                var fieldType = field.FieldType;
                Action<GameObject> bindAction = (x) => SetFieldValue(skill, field, x);
                OnObjectMouseDown = bindAction;
                //do{
                    yield return new WaitUntil(() => field.GetValue(skill) != null || skill != selectedSkill);
                

                //} while (!skill.IsActivable());

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
        controller.CreateCommand(skill);
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
}
