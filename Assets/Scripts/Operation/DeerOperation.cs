using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerOperation : Operation
{
    public Entity selectEntity;
    public Tile selectTile;
    
    private void Start()
    {
        markers = new List<GameObject>();
    }

    public override void GetOperationValue()
    {
        selectEntity = controller.selectedEntity;
        selectTile = controller.selectedTile;
    }

    public override void OperatorActivate()
    {
        Debug.Log("cell : " + selectTile.name + " : " + selectEntity.name);
        if (selectEntity != null && selectTile != null)
        {
            selectEntity.MoveToTile(selectTile);
        }
        selectTile = null;
        selectEntity = null;
        Destroy(gameObject);
    }
}
