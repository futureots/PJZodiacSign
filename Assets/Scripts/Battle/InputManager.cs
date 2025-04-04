using Battle;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EntityController;

public class InputManager : Singleton<InputManager>
{
    public EntityController controller;

    //false일때 입력을 받고 true이면 입력을 받지 않음
    public bool isInputStop;
    public Battle.Entity selectedEntity;
    public Tile selectedTile;

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
    public bool isEntitySelect
    {
        get
        {
            if (controller == null) return false;
            return controller.isEntitySelected;
        }
    }
    public bool isTileSelect
    {
        get
        {
            if (controller == null) return false;
            return controller.isTileSelected;
        }
    }
    public GameObject entitySelecter;
    public GameObject tileSelecter;
    public void OnGameObjectDown(GameObject selectObj)
    {
        if (isInputStop) return;
        var entity = selectObj.GetComponent<Battle.Entity>();
        if (entity == null) return;

        selectedEntity = entity;
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
        //해당 입력에 대한 컨트롤러 커맨드 작성
        selectedEntity.transform.position = selectedEntity.curTile.transform.position;
        selectedEntity = null;
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

        ShowEntitySelecter();
        ShowCellSelecter();
    }
    void ShowEntitySelecter()
    {
        if (isEntitySelect)
        {
            if (!entitySelecter.activeSelf)
            {
                entitySelecter.SetActive(true);
                entitySelecter.transform.localScale = Vector3.one * controller.selectedEntity.transform.lossyScale.y;
            }
            entitySelecter.transform.position = new Vector3(controller.selectedEntity.transform.position.x, 0.1f, controller.selectedEntity.transform.position.z);
        }
        else if (entitySelecter.activeSelf)
        {
            entitySelecter.SetActive(false);
        }
    }
    void ShowCellSelecter()
    {
        if (isTileSelect)
        {
            if (!tileSelecter.activeSelf)
            {
                tileSelecter.SetActive(true);
                tileSelecter.transform.localScale = Vector3.one * controller.selectedTile.transform.lossyScale.y * 4;
            }
            tileSelecter.transform.position = new Vector3(controller.selectedTile.transform.position.x, 0.1f, controller.selectedTile.transform.position.z);
        }
        else if (tileSelecter.activeSelf)
        {
            tileSelecter.SetActive(false);
        }
    }
    
}
