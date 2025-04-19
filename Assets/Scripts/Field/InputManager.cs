using System;
using UnityEngine;
using Battle;
using System.Reflection;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;

public class InputManager : Singleton<InputManager>
{
    public EntityController controller;

    //false일때 입력을 받고 true이면 입력을 받지 않음
    public bool isInputStop;

    public AreaVisualizer areaVisualizer;

    public static event Action<GameObject> OnObjectMouseDown;
    public static event Action OnObjectMouseUp;

    public GameObject entitySelecter;
    public GameObject tileSelecter;
    private void Start()
    {
        StartCoroutine(SelecterUpdate());
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
    public Battle.Entity selectedEntity;

    GameObject targetSelecter;
    GameObject targetTileSelecter;
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
        targetSelecter = Instantiate(entitySelecter, selectedEntity.transform.position + Vector3.up * 0.1f, Quaternion.identity);
        targetTileSelecter = Instantiate(tileSelecter, selectedEntity.transform.position, Quaternion.identity);
        areaVisualizer.ShowMoveArea(entity.GetMoveArea());
    }
    IEnumerator SelecterUpdate()
    {
        while (true) {
            yield return new WaitUntil(() => selectedEntity != null);
            var closeTile = GetClosestTile(selectedEntity.transform.position, selectedEntity.GetMoveArea());
            targetTileSelecter.transform.position = closeTile.transform.position + Vector3.up * 0.1f;

            List<Tile> list = selectedEntity.GetAttackArea(closeTile);
            areaVisualizer.ShowAttackArea(list);
            yield return new WaitForFixedUpdate();
            areaVisualizer.RemoveAttackArea(list);

        }
    }
    /// <summary>
    /// 선택한 Entity 제거 및 명령 전달
    /// </summary>
    void ReleaseEntity()
    {
        if (selectedEntity != null)
        {
            var tile = GetClosestTile(selectedEntity.transform.position, selectedEntity.GetMoveArea());
            controller.CreateCommand(selectedEntity, tile);
            selectedEntity.transform.position = selectedEntity.curTile.transform.position;

            areaVisualizer.RemoveMoveArea(selectedEntity.GetMoveArea());
            
            Destroy(targetSelecter);
            Destroy(targetTileSelecter);

            selectedEntity = null;
        }
    }
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
