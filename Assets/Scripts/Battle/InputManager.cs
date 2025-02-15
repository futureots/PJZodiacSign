using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityController;

public class InputManager : Singleton<InputManager>
{
    public EntityController playerController;
    public enum InputMode
    {
        //명령 없음(행동 X)
        None,
        //초기 기물 세팅용
        Set,
        //기물 이동(기물 범위 내 타일만 선택가능)
        Move
    }
    public InputMode inputMode;
    public bool isEntitySelect
    {
        get
        {
            if (playerController == null) return false;
            return playerController.isEntitySelected;
        }
    }
    public bool isTileSelect
    {
        get
        {
            if (playerController == null) return false;
            return playerController.isTileSelected;
        }
    }
    public GameObject entitySelecter;
    public GameObject tileSelecter;
    public void OnEntityDown(Entity entity)
    {
        switch (inputMode)
        {
            case InputMode.None:
                break;
            case InputMode.Move:
            case InputMode.Set:
                playerController.EntitySelect(entity);
                break;
            default:
                break;
        }
        
    }
    public void OnEntityDrag(Entity entity)
    {
        switch (inputMode)
        {
            case InputMode.None:
                break;
            case InputMode.Set:
                playerController.EntitySetDrag(entity);
                break;
            case InputMode.Move:
                playerController.EntityMoveDrag(entity);
                break;
            default:
                break;
        }
    }
    public void OnEntityUp(Entity entity)
    {
        switch (inputMode)
        {
            case InputMode.None:
                break;
            case InputMode.Set:
                playerController.EntitySetUp(entity);
                break; 
            case InputMode.Move:
                playerController.EntityMoveUp(entity);
                break;
            default:
                break;
        }
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
        ShowEntitySelecter();
        ShowCellSelecter();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.SaveCurrentState(playerController.teamNum);
        }
    }
    void ShowEntitySelecter()
    {
        if (isEntitySelect)
        {
            if (!entitySelecter.activeSelf)
            {
                entitySelecter.SetActive(true);
                entitySelecter.transform.localScale = Vector3.one * playerController.selectedEntity.transform.lossyScale.y;
            }
            entitySelecter.transform.position = new Vector3(playerController.selectedEntity.transform.position.x, 0.1f, playerController.selectedEntity.transform.position.z);
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
                tileSelecter.transform.localScale = Vector3.one * playerController.selectedTile.transform.lossyScale.y * 4;
            }
            tileSelecter.transform.position = new Vector3(playerController.selectedTile.transform.position.x, 0.1f, playerController.selectedTile.transform.position.z);
        }
        else if (tileSelecter.activeSelf)
        {
            tileSelecter.SetActive(false);
        }
    }
    
}
