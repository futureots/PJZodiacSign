using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityController : MonoBehaviour
{
    public bool isReflect;
    //public Field currentField;



    private void Start()
    {
        entities = new List<Entity>();
        // Debug
        var myEntity = Instantiate(entityObj);
        myEntity.tag = this.tag;
        entities.Add(myEntity);
        myEntity.MoveTo(Field.Instance.GetTile(entityPos));
        
    }

    #region EntityManaging

    // 컨트롤러가 조종 가능한 엔티티
    List<Entity> entities;
    // 디버그용 엔티티 스폰 위치(나중에 데이터에서 불러와 위치 지정 기능 추가)
    public Entity entityObj;
    public intVector2 entityPos;
    public bool IsContainEntity(Entity entity)
    {
        return entities.Contains(entity);
    }


    #endregion
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

    #region Command

    public Command curCmd
    {
        get
        {
            return _curCmd;
        }
        set
        {
            _curCmd?.Delete();
            _curCmd = value;
        }
    }
    Command _curCmd;

    public Command CreateCommand(Entity entity, Tile tile, params GameObject[] selecter)
    {
        Command cmd = new MoveCommand(entity, tile);
        curCmd = cmd;
        cmd.selecterObjects.AddRange(selecter);
        return cmd;
    }

    public Command CreateCommand(ISkill skill, params GameObject[] selecter)
    {
        Command cmd = new SkillCommand(skill);
        curCmd = cmd;
        cmd.selecterObjects.AddRange(selecter);
        return cmd;
    }

    #endregion
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