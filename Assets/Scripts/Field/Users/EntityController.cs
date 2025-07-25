using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EntityController : MonoBehaviour
{
    // 필드를 바라보는 방향
    public bool isReflect;
    
    // 현재 보유 기물 필드(설치 X)
    public Field instantField;

    // 컨트롤러가 조종 가능한 엔티티
    List<Entity> entities;
    // 컨트롤러 팀
    [SerializeField]Team team;
    private void Awake()
    {
        entities = new List<Entity>();
        team = GetComponent<Team>();
    }

    #region EntityManaging
    public bool PlaceEntity(Entity instance, Field field)
    {
        for (int i = field.row - 1; i >= 0; i--) 
        {
            for(int j = 0; j < field.column; j++)
            {
                var pos = new intVector2(j, i);
                var tile = field.GetTile(pos);
                if (tile.isEmpty)
                {
                    PlaceEntity(instance, field, pos);
                    return true;
                }
            }
        }
        return false;
    }
    public bool PlaceEntity(Entity instance, Field field, intVector2 pos)
    {
        var movable = instance.MoveSequence(field.GetTile(pos), true);
        if (movable)
        {
            instance.GetOrAddComponent<Team>().teamNumber = team.teamNumber;
            entities.Add(instance);
        }
        return movable;
    }
    public void SetInstantField(List<EntityLevelData> handEntities)
    {
        instantField.gameObject.SetActive(true);
        foreach (var item in handEntities)
        {
            var entity = item.data.CreateEntity(item.level);
            PlaceEntity(entity, instantField);
        }
    }
    public virtual void SetMainField(Dictionary<int,EntityLevelData> fieldEntities)
    {

    }
    public virtual void DisposeInstantField()
    {
        Debug.Log("DIsposeInstantField");
        instantField.EraseField();
        instantField.gameObject.SetActive(false);
    }

    #endregion

    #region Data
    public (Dictionary<int,EntityLevelData>, List<EntityLevelData>) GetFieldData()
    {
        // 메인 필드 데이터 가져오기
        var tiles = GameManager.Instance.field.GetHalfTiles(isReflect);
        Dictionary<int, EntityLevelData> mainFieldData = new Dictionary<int, EntityLevelData>();
        foreach (var tile in tiles)
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            var entityData = new EntityLevelData(entity);
            int pos = tile.fieldPos.Encode();
            mainFieldData.Add(pos, entityData);
        }

        //인스턴트 필드 데이터 가져오기
        List<EntityLevelData> instantFieldData = new List<EntityLevelData>();
        foreach (var tile in instantField.GetTiles())
        {
            if (tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            var entityData = new EntityLevelData(entity);
            instantFieldData.Add(entityData);
        }
        return (mainFieldData, instantFieldData);
    }
    #endregion

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
    public Command CreateCommand(IActive skill, params GameObject[] selecter)
    {
        Command cmd = new SkillCommand(skill);
        curCmd = cmd;
        cmd.selecterObjects.AddRange(selecter);
        return cmd;
    }
    public void ClearCommand()
    {
        curCmd = null;
    }

    #endregion

}