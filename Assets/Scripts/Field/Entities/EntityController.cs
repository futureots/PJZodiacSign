using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EntityController : MonoBehaviour
{
    // 필드를 바라보는 방향
    public bool isReflect;
    
    // 현재 보유 기물 필드(설치 X)
    public Field instantField;

    // 컨트롤러가 조종 가능한 엔티티
    List<Entity> entities;
    [SerializeField]Team team;
    private void Awake()
    {
        entities = new List<Entity>();
        team = GetComponent<Team>();
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
        var tiles = GameManager.Instance.field.GetHalfTiles(isReflect);
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
    public bool PushEntity(Entity instance, Field field)
    {
        for (int i = field.row - 1; i >= 0; i--) 
        {
            for(int j = 0; j < field.column; j++)
            {
                var pos = new intVector2(j, i);
                var tile = field.GetTile(pos);
                if (tile.isEmpty)
                {
                    SetEntity(instance, field, pos);
                    return true;
                }
            }
        }
        return false;
    }
    public bool SetEntity(Entity instance, Field field, intVector2 pos)
    {
        var movable = instance.MoveSequence(field.GetTile(pos), true);
        if (movable)
        {
            instance.GetOrAddComponent<Team>().teamNumber = team.teamNumber;
            entities.Add(instance);
        }
        return movable;
    }
    public virtual void SetInstantField()
    {
        instantField.gameObject.SetActive(true);
    }
    public virtual void SetMainField()
    {

    }
    public virtual void DisposeInstantField()
    {
        instantField.EraseField();
        instantField.gameObject.SetActive(false);
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