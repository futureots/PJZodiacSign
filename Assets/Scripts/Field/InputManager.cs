using System;
using UnityEngine;
using Battle;
using System.Reflection;

public class InputManager : Singleton<InputManager>
{
    public EntityController controller;

    //false일때 입력을 받고 true이면 입력을 받지 않음
    public bool isInputStop;
    public Battle.Entity selectedEntity;
    public Skill selectedSkill;

    public static event Action<GameObject> OnObjectMouseDown;
    public static event Action OnObjectMouseUp;
    #region inputMode
    public enum InputMode
    {
        //명령 없음(행동 X)
        None=-1,
        //초기 기물 세팅용
        Set=0,
        //기물 이동(기물 범위 내 타일만 선택가능)
        Move=1
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
        if(selectedEntity != null)
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
        //SelectEntity(selectObj);
        /*
        switch (inputMode)
        {
            case InputMode.None:
                break;
            case InputMode.Move:
            case InputMode.Set:
                playerController.SelectEntity(entity,(int)inputMode);
                break;
            default:
                break;
        }*/
    }


    public void OnGameObjectUp()
    {
        if (isInputStop) return;
        OnObjectMouseUp?.Invoke();
        //ReleaseEntity();
        /*
        switch (inputMode)
        {
            case InputMode.None:
                break;
            case InputMode.Set:
                playerController.MouseUpEntity(entity,true);
                break; 
            case InputMode.Move:
                playerController.MouseUpEntity(entity);
                break;
            default:
                break;
        }*/
    }
    #region MoveCommand관련
    public void AllocateMoveCommand()
    {
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
    }
    /// <summary>
    /// 선택한 Entity 제거 및 명령 전달
    /// </summary>
    void ReleaseEntity()
    {
        Tile tile = controller.GetClosestTile(selectedEntity.transform.position, GameManager.Instance.field);
        controller.CreateCommand(selectedEntity, tile);
        //해당 입력에 대한 컨트롤러 커맨드 작성
        

        selectedEntity.transform.position = selectedEntity.curTile.transform.position;
        selectedEntity = null;
    }
    #endregion
    #region SkillCommand 관련
    public void AllocateSkillCommand(Skill skill)
    {
        OnObjectMouseDown = null;
        OnObjectMouseUp = null;
    }

    void SetFieldValue(Skill skill, FieldInfo field, GameObject obj)
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
