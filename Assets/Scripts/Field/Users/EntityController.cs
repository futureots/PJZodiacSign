using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    // 필드를 거울처럼 보는 방향
    public bool isReflect;
    
    // 손에 들고 있는 기물 필드(위치 X)
    public Field instantField;

    // 팀에 속한 모든 기물들
    public List<Entity> entities;
    // 팀 번호
    [SerializeField]Team team;
    private void Awake()
    {
        entities = new List<Entity>();
        team = GetComponent<Team>();
    }

    #region EntityManaging
    /// <summary>
    /// 기물을 손에 들고 있는 필드에 배치하는 함수
    /// </summary>
    /// <param name="instance">기물 인스턴스</param>
    /// <returns>배치 성공 시 true, 실패 시 false 반환</returns>
    public bool PlaceOnInstantField(Entity instance)
    {
        for (int i = instantField.row - 1; i >= 0; i--)
        {
            for (int j = 0; j < instantField.column; j++)
            {
                var pos = new intVector2(j, i);
                var tile = instantField.GetTile(pos);
                if (tile.isEmpty)
                {
                    PlaceEntity(instance, tile);
                    return true;
                }
            }
        }
        return false;
    }
    public bool PlaceOnMainField(Entity instance, intVector2 pos)
    {
        var tile = GameManager.Instance.field.GetTile(pos,isReflect);
        return PlaceOnMainField(instance, tile);
    }
    public bool PlaceOnMainField(Entity instance, Tile tile)
    {
        if (tile.isEmpty)
        {
            PlaceEntity(instance, tile);
            return true;
        }
        return false;
    }
    public void PlaceEntity(Entity instance, Tile tile)
    {
        instance.MoveTo(tile);
        instance.isReflect = isReflect;

        instance.sourceField = tile.field;

        instance.OnDead += () =>
        {
            entities.Remove(instance);
        };
        instance.GetOrAddComponent<Team>().teamNumber = team.teamNumber;
    }
    public void SetInstantField(List<EntityLevelData> handEntities)
    {
        instantField.gameObject.SetActive(true);
        foreach (var item in handEntities)
        {
            var entity = item.data.CreateEntity(item.level);
            PlaceOnInstantField(entity);
        }
    }
    public void SetMainField(Dictionary<int,EntityLevelData> fieldEntities)
    {
        foreach (var item in fieldEntities)
        {
            var entity = item.Value.data.CreateEntity(item.Value.level);
            intVector2 pos = intVector2.Decode(item.Key);
            PlaceOnMainField(entity, pos);
            
        }
    }
    public virtual void DisposeInstantField()
    {
        Debug.Log("DIsposeInstantField");
        instantField.ResetField();
        instantField.gameObject.SetActive(false);
    }

    public void UpdateEntities()
    {
        var team = GetComponent<Team>();
        entities.Clear();
        foreach(var tile in GameManager.Instance.field.GetTiles())
        {
            if(tile.isEmpty) continue;
            var entity = tile.occupiedObject.GetComponent<Entity>();
            if(entity == null) continue;
            if (team.isAlly(entity.team))
            {
                entities.Add(entity);
            }
        }
    }
    #endregion

    #region Data
    public (Dictionary<int,EntityLevelData>, List<EntityLevelData>) GetFieldData()
    {
        // 메인 필드 데이터를 딕셔너리로 변환
        Dictionary<int, EntityLevelData> mainFieldData = new Dictionary<int, EntityLevelData>();
        UpdateEntities();
        foreach (var entity in entities)
        {
            var entityData = new EntityLevelData(entity);
            int pos = entity.curTile.fieldPos.Encode();
            mainFieldData.Add(pos, entityData);
        }

        //인스턴트 필드 데이터를 리스트로 변환
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
    // 현재 생성된 커맨드 
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
            OnCommandCreated?.Invoke(_curCmd);
        }
    }
    Command _curCmd;
    public Action<Command> OnCommandCreated;

    // 이동 커맨드 생성
    public Command CreateCommand(Entity entity, Tile tile, params GameObject[] selecter)
    {
        Command cmd = new MoveCommand(entity, tile);
        cmd.selecterObjects.AddRange(selecter);
        curCmd = cmd;
        return cmd;
    }
    // 스킬 커맨드 생성
    public Command CreateCommand(IActive skill, params GameObject[] selecter)
    {
        Command cmd = new SkillCommand(skill);
        cmd.selecterObjects.AddRange(selecter);
        curCmd = cmd;
        return cmd;
    }
    public void ClearCommand()
    {
        curCmd = null;
    }

    #endregion

}