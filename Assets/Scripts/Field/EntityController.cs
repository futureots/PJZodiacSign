using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;
using Battle;

public class EntityController : MonoBehaviour
{
    [SerializeField] JodiacSO jodiacList;
    public bool isReflect;

    public Material expectAttackMaterial;
    
    public Material expectMoveMaterial;

    //public Field currentField;
    public int teamNum;
    public List<Oldity> entities;

    public Command curCmd;

    // 데이터 기반 엔티티 설정 및 세팅
    /*public virtual void SetEntities(int num, PlayerData party)
    {
        teamNum = num;
        int x = -35;
        foreach (var member in party.entities)
        {
            var entity = CreateEntity(member.entityId, member.entityElement);
            entities.Add(entity);
            entity.SetEntityData(teamNum, member.entityLevel, isReflect);
            entity.OnDestroyed += (entity) =>
            {
                entities.Remove(entity);
            };
            HpPanelManager.Instance.CreateHpBar(entity.gameObject);
            var reflectVariable = isReflect ? -1 : 1;
            entity.transform.position = new Vector3(-45*reflectVariable, 0, x*reflectVariable);
            x += 10;
        }
    }
    Entity CreateEntity(Jodiac jodiac, Element element)
    {
        var entityData = jodiacList.GetJodiac(jodiac);
        var entityObj = Instantiate(entityData);
        var entity = entityObj.GetComponent<Entity>();
        entity.tag = tag;
        //entity.elementType = element;
        //entity.field = currentField;
        return entity;
    }*/
    public Command CreateCommand(Entity entity, Tile tile)
    {
        Command cmd = new MoveCommand(entity, tile);
        curCmd = cmd;
        return cmd;
    }
    /*public Command GetRandomCommand()
    {   
        var entityList = new List<Entity>(entities);
        while (entityList.Count > 0) 
        {
            var owner = entityList[Random.Range(0, entityList.Count)];
            var area = owner.GetMoveArea(owner.curPos, false);
            if(area.Count > 0)
            {
                Tile destination = area[Random.Range(0,area.Count)];
                MoveCommand cmd = new MoveCommand(owner, destination);
                return cmd;
            }
            else
            {
                entityList.Remove(owner);
            }
        }
        // 이동 가능한 영물 없음
        return null;
    }*/
    #region EntitySelectInput

    public Tile GetClosestTile(Vector3 pos,Field field)
    {
        if (field == null) return null;
        float minDistance = 0;
        Tile closestTile = null;
        foreach (Tile tile in field.GetTiles())
        {
            //if (tile.isOccupied) continue;
            var distance = (tile.transform.position - pos).magnitude;
            if (closestTile == null || minDistance > distance)
            {
                minDistance = distance;
                closestTile = tile;
            }
        }
        return closestTile;
    }

    List<Tile> movableTile = new List<Tile>();
    List<Tile> attackableTile = new List<Tile>();
    // 엔티티 선택
    /*public void SelectEntity(Entity entity, int mode)
    {
        if (!entities.Contains(entity)) return;
        entity.field = currentField;
        selectedEntity = entity;
        switch (mode)
        {
            case 0:
                movableTile.AddRange(currentField.GetHalfTiles(isReflect));
                break;
            case 1:
                movableTile.AddRange(entity.GetMoveArea(entity.curPos));
                break;
            default:
                break;
        }
        currentField.AddFieldColor(0, movableTile.ToArray());
        GameManager.Instance.ViewAttackArea(entity.TeamNum);
    }*/
    /*public void DragEntity(Entity entity)
    {
        //팀이 아니면 이동 권한 없음
        if (!entities.Contains(entity)) return;
        if (selectedEntity != entity) selectedEntity = entity;
        //기물 마우스 위치로 이동
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, 10, 0));
        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            Vector3 pos = ray.GetPoint(rayDistance);
            entity.transform.position = pos;
        }
        Tile closestTile = GetClosestTile(entity.transform.position, movableTile);
        if (selectedTile != closestTile && closestTile != null)
        {
            if (selectedTile != null)
            {
                currentField.RemoveFieldColor(1);
                attackableTile.Clear();
            }
            selectedTile = closestTile;
            var area = entity.GetAttackArea(selectedTile.fieldPos);
            attackableTile.AddRange(area);
            currentField.AddFieldColor(1, attackableTile.ToArray());
        }

    }*/
    /*public void MouseUpEntity(Entity entity, bool isSet = false)
    {
        if (!entities.Contains(entity)) return;
        //Debug.Log(selectedEntity.curTile);
        if (isSet)
        {
            if (selectedTile != null)
            {
                if (selectedTile == entity.curTile)
                {
                    Debug.Log("Not Move");
                }
                entity.MoveToTile(selectedTile, false);
                selectedTile = null;
                selectedEntity = null;
            }
        }
        else
        {
            selectedEntity.transform.position = selectedEntity.curTile.transform.position;
            if (selectedTile == selectedEntity.curTile)
            {
                selectedEntity = null;
                selectedTile = null;
            }
        }

        currentField.RemoveFieldColor(1);
        attackableTile.Clear();
        currentField.RemoveFieldColor(0);
        movableTile.Clear();
        GameManager.Instance.ClearAttackArea();
    }*/

    #endregion
}