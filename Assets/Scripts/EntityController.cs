using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityController : Singleton<EntityController>
{
    public bool isReflect;
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
    //private로 변경 시 리스트 할당 필요
    public List<Entity> entities;
    
    //처음 한번만 실행되는 함수(여야함)
    public void SetEntities(int num, PartyData party)//+ 지점, 엔티티를 가진 구조체 리스트를 가진 클래스 받아오기 => 해당 지점에 해당 엔티티 소환 후 팀 넘버 설정
    {
        teamNum = num;
        foreach (var member in party.Entities)
        {
            var prefab = Resources.Load<GameObject>("Pieces/"+ member.entityId);
            var obj = Instantiate(prefab);
            Entity entity = obj.GetComponent<Entity>();
            entities.Add(entity);
            entity.field = currentField;
            entity.teamNum = num;
            entity.isReflect = isReflect;
            entity.OnDestroyed += (entity) =>
            {
                entities.Remove(entity);
            };
            var fieldPos = member.pos;
            if (isReflect)
            {
                fieldPos = new intVector2(7, 7) - fieldPos;
            }
            entity.MoveToTile(currentField.GetTile(fieldPos));
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
                        if(!tile.isOccupied) selectedTile = selectedTile == tile ? null : tile;
                    }
                }
                break;
            case EMode.SelectBoth:
                if(selectedEntity != null)
                {
                    if(!tile.isOccupied) selectedTile = selectedTile == tile ? null : tile;
                }
                break;
            default:
                break;
        }        
    }
    public void OperActivate()
    {
        
        //시간내로 입력하지 않아서 정보값이 부족할 경우 이동 가능한 랜덤 기물 1개가 이동가능한 무작위 타일로 이동
        if(selectedEntity == null || selectedTile == null)
        {
            //컨트롤러가 가지고 있는 엔티티 복사
            var tempList = new List<Entity>(entities);
            while (true)
            {
                //움직일 수 있는 기물이 없을 경우 종료
                if (tempList.Count <= 0)
                {
                    Debug.Log("Cant Move");
                    break;
                }
                var tempEntity = tempList[Random.Range(0, tempList.Count)];
                var area = tempEntity.GetEntityArea();
                //해당 기물이 이동가능한 칸이 있는지 확인
                bool movable = false;
                foreach (var item in area)
                {
                    if (currentField.IsMovable(item))
                    {
                        movable = true;
                        selectedTile = currentField.GetTile(item);
                        Debug.Log("POS : " + item.x + item.y);
                        break;
                    }
                }
                //이동가능하면 기물 지정
                if (movable)
                {
                    
                    selectedEntity = tempEntity;
                    break;
                }
                //이동 불가능 시 해당 기물 빼고 재시도
                else
                {
                    tempList.Remove(tempEntity);
                    Debug.Log("tempList.Count : " + tempList.Count);
                }

            }
        }
        //지정한 기물과 칸이 있으면 실행
        if (selectedEntity != null && selectedTile != null)
        {
            //Debug.Log(name+"cell : " + selectedTile.name + " : " + selectedEntity.name);
            selectedEntity.MoveToTile(selectedTile);
        }
        selectedTile = null;
        selectedEntity = null;
    }
}