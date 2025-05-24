using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityController : MonoBehaviour
{
    // 필드를 바라보는 방향
    public bool isReflect;
    //public Field currentField;

    // 컨트롤러가 조종 가능한 엔티티
    List<Entity> entities;

    private void Awake()
    {
        entities = new List<Entity>();
        
    }

    #region EntityManaging

    /// <summary>
    /// 해당 엔티티를 자신의 소유 및 자신의 필드에 랜덤 스폰
    /// </summary>
    /// <param name="entityInstance"></param>
    public void SetEntity(Entity entityInstance)
    {
        entityInstance.tag = this.tag;
        entities.Add(entityInstance);

        // 필드의 랜덤 위치로 이동
        var tiles = Field.Instance.GetHalfTiles(isReflect);
        while (true)
        {
            var rand = Random.Range(0, tiles.Count);
            if (tiles[rand].isEmpty)
            {
                entityInstance.MoveTo(tiles[rand]);
                break;
            }
        }
        
    }

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
    */

    #region Command
    // 현재 저장된 명령
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

    // 이동 명령 생성
    public Command CreateCommand(Entity entity, Tile tile, params GameObject[] selecter)
    {
        Command cmd = new MoveCommand(entity, tile);
        curCmd = cmd;
        cmd.selecterObjects.AddRange(selecter);
        return cmd;
    }
    // 스킬 명령 생성
    public Command CreateCommand(ISkill skill, params GameObject[] selecter)
    {
        Command cmd = new SkillCommand(skill);
        curCmd = cmd;
        cmd.selecterObjects.AddRange(selecter);
        return cmd;
    }

    #endregion
    // 해당 위치에서 가장 가까운 타일을 반환한다.
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