using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityController : Singleton<EntityController>
{
    public Entity selectedEntity { get; private set; }
    public Tile selectedTile {  get; private set; }
    public bool isEntitySelected => selectedEntity != null;
    public bool isTileSelected => selectedTile != null;
    public enum EMode
    {
        //기물 선택
        SelectEntity,
        //지점 선택
        SelectTile,
        //기물 지점 선택
        SelectBoth,
        SelectRelativeCell,
        //필드 확인
        FieldSearch,
    }
    //턴 시작 시 필드 확인을 기본으로 정하며, 기물을 누르면 기물 행동으로 바뀌고 스킬 아이콘을 누르면 스킬 사용 모드로 바뀜
    EMode currentMode;
    public EMode mode { get { return currentMode; } 
        set
        {
            currentMode = value;
            selectedEntity = null;
            selectedTile = null;
        } 
    }
    public Field currentField;
    public void StartMovePhase()
    {
        mode = EMode.SelectRelativeCell;
    }
    
    public int teamNum;
    public List<Entity> entities;
    public void SetEntities(int num, PartyData party)//+ 지점, 엔티티를 가진 구조체 리스트를 가진 클래스 받아오기 => 해당 지점에 해당 엔티티 소환 후 팀 넘버 설정
    {
        teamNum = num;
        foreach (var member in party.Entities)
        {
            var prefab = Resources.Load<GameObject>("Pieces/"+ member.entityId);
            var obj = Instantiate(prefab);
            Entity entity = obj.GetComponent<Entity>();
            entity.field = currentField;
            entity.MoveToTile(currentField.GetTile(member.pos));
        }
    }
    private void Start()
    {
        StartMovePhase();
    }
    public void OnClickEntity(Entity entity)
    {
        if (entity == null) return;
        Debug.Log(entity);
        switch (mode)
        {
            case EMode.SelectEntity:
            case EMode.FieldSearch:
                selectedTile = null;
                selectedEntity = selectedEntity == entity ? null : entity;
                break;
            case EMode.SelectBoth:
            case EMode.SelectRelativeCell:
                selectedTile = null;
                selectedEntity = selectedEntity == entity ? null : entity.teamNum == teamNum ? entity : null;                                                 //팀이 같은 경우에만 선택 가능
                if (selectedEntity == null) selectedTile = null;
                break;
            case EMode.SelectTile:
                OnClickTile(entity.curTile);
                break;
            default:
                break;
        }
    }
    public void OnClickTile(Tile tile)
    {
        if(tile == null) return;
        Debug.Log(tile);
        switch (mode)
        {
            //정보 표출을 위한 타일 하나 입력
            case EMode.SelectTile:
            case EMode.FieldSearch:
                selectedEntity = null;
                selectedTile = selectedTile == tile ? null : tile;
                break;
            case EMode.SelectEntity:
                var entity = tile.GetEntity();
                if (entity != null)
                {
                    OnClickEntity(entity);
                }
                break;
            case EMode.SelectRelativeCell:
                if(selectedEntity != null)
                {
                    if (selectedEntity.IsInArea(tile.fieldPos))
                    {
                        if(!tile.isAttackable) selectedTile = selectedTile == tile ? null : tile;
                    }
                }
                break;
            case EMode.SelectBoth:
                if(selectedEntity != null)
                {
                    if(!tile.isAttackable) selectedTile = selectedTile == tile ? null : tile;
                }
                break;
            default:
                break;
        }        
    }
    public void OperActivate()
    {
        Debug.Log("cell : " + selectedTile.name + " : " + selectedEntity.name);
        if (selectedEntity != null && selectedTile != null)
        {
            selectedEntity.MoveToTile(selectedTile);
        }
        else
        {
            while (true)
            {
                var randEntity = entities[Random.Range(0, entities.Count)];
                if (randEntity == null) break;
                var area = randEntity.GetEntityArea();
                var randTile = currentField.GetTile(area[Random.Range(0, area.Count)]);
                if (randTile == null)
                {
                    continue;
                }
            }


        }
        selectedTile = null;
        selectedEntity = null;


        //moveOperation = null;
    }
}