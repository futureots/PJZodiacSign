using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;
using Battle;

public class EntityController : MonoBehaviour
{
    public bool isReflect;
    //public Field currentField;
    public int teamNum;
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
    public Command CreateCommand(ISkill skill)
    {
        Command cmd = new SkillCommand(skill);
        curCmd = cmd;
        Debug.Log("Command Created");
        return cmd;
    }

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
}