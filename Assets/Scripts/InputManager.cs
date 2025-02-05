using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityController;

public class InputManager : Singleton<InputManager>
{
    public EntityController playerController;
    
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
    public void EntityInput(Entity entity)
    {
        playerController.OnClickEntity(entity);
    }
    public void TileInput(Tile tile)
    {
        playerController.OnClickTile(tile);
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
