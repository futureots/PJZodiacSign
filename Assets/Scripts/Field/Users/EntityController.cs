using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    // 필드를 거울처럼 보는 방향
    public bool isReflect;
    
    // 리소스 필드(위치 X)
    public Field resourceField;

    // 팀에 속한 모든 기물들
    public List<Entity> fieldEntities { get; private set; }
    void AddFieldEntity(Entity entity)
    {
        if (fieldEntities.Contains(entity)) return;
        fieldEntities.Add(entity);
        entity.onDead += () =>
        {
            fieldEntities.Remove(entity);
        };
    }
    public List<Entity> resourceEntities { get; private set; }

    // 팀 번호
    [SerializeField]Team team;
    private void Awake()
    {
        fieldEntities = new List<Entity>();
        resourceEntities = new List<Entity>();

        //team = GetComponent<Team>();
    }


    #region EntityManaging

    /// <summary>
    /// 기물을 리소스 필드에 초기 배치하는 함수
    /// </summary>
    /// <param name="instance">기물 인스턴스</param>
    /// <returns>배치 성공 시 true, 실패 시 false 반환</returns>
    public bool PlaceOnResourceField(Entity instance)
    {
        var list = Field.GetEmptyTiles(resourceField.GetTiles());
        if (list.Count <= 0)
        {
            return false;
        }
        PlaceEntity(instance, list[0]);
        resourceEntities.Add(instance);
        return true;
    }

    public void PlaceEntity(Entity instance, Tile tile)
    {
        instance.Move(tile, true);
        instance.IsReflect = isReflect;

        instance.GetOrAddComponent<Team>().teamNumber = team.teamNumber;
    }


    public void SetResourceField(List<EntityLevelData> handData)
    {
        resourceEntities.Clear();
        resourceField.gameObject.SetActive(true);

        foreach (var item in handData)
        {
            var entity = EntityFactory.CreateEntity(item.data, item.level);
            if (!PlaceOnResourceField(entity))
            {
                Destroy(entity);
            }
        }
    }
    public void SetMainField(Dictionary<intVector2,EntityLevelData> fieldData)
    {
        // fieldEntities.Clear();
        //
        // foreach (var item in fieldData)
        // {
        //     var entity = EntityFactory.CreateEntity(item.Value.data, item.Value.level);
        //     intVector2 pos = item.Key;
        //
        //     var tile = GameManager.Instance.field.GetTile(pos, isReflect);
        //     PlaceEntity(entity, tile);
        //
        //     entity.GetOrAddComponent<Team>().teamNumber = team.teamNumber;
        //
        //     AddFieldEntity(entity);
        // }
    }
    public virtual void DisposeInstantField()
    {
        resourceField.ResetField();
        resourceField.gameObject.SetActive(false);
    }

    public void UpdateEntities()
    {
        var team = GetComponent<Team>();

        foreach (var e in resourceEntities)
        {
            if (e == null) continue;
            if(e.CurTile.field != resourceField)
            {
                AddFieldEntity(e);
            }
        }
        resourceEntities.RemoveAll((e) => e.CurTile.field != resourceField);
    }
    #endregion

    #region Data
    public (Dictionary<intVector2,EntityLevelData>, List<EntityLevelData>) GetFieldData()
    {
        // 메인 필드 데이터를 딕셔너리로 변환
        Dictionary<intVector2, EntityLevelData> mainFieldData = new Dictionary<intVector2, EntityLevelData>();
        UpdateEntities();
        foreach (var entity in fieldEntities)
        {
            var entityData = new EntityLevelData(entity);
            intVector2 pos = entity.CurTile.fieldPos;
            mainFieldData.Add(pos, entityData);
        }

        //인스턴트 필드 데이터를 리스트로 변환
        List<EntityLevelData> instantFieldData = new List<EntityLevelData>();
        foreach (var tile in resourceField.GetTiles())
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
            _curCmd?.DeleteObjects();
            _curCmd = value;
            onCommandCreated?.Invoke(_curCmd);
        }
    }
    Command _curCmd;
    public Action<Command> onCommandCreated;

    // 이동 커맨드 생성
    public Command CreateCommand(Entity entity, Tile tile, params GameObject[] selecter)
    {
        Command cmd = new MoveCommand(entity, tile);
        cmd.selecterObjects.AddRange(selecter);
        curCmd = cmd;
        return cmd;
    }
    // 스킬 커맨드 생성
    public Command CreateCommand(SkillComponent skill, params GameObject[] selecter)
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