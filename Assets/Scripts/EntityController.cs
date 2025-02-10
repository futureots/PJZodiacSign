using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityController : MonoBehaviour
{
    public bool isReflect;
    public Entity selectedEntity { get; private set; }
    public Tile selectedTile {  get; private set; }
    public bool isEntitySelected => selectedEntity != null;
    public bool isTileSelected => selectedTile != null;
    public Field currentField;
    public int teamNum;
    //private로 변경 시 리스트 할당 필요
    public List<Entity> entities;
    
    //처음 한번만 실행되는 함수(여야함)
    public virtual void SetEntities(int num, PartyData party)//+ 지점, 엔티티를 가진 구조체 리스트를 가진 클래스 받아오기 => 해당 지점에 해당 엔티티 소환 후 팀 넘버 설정
    {
        teamNum = num;
        int x = -35;
        foreach (var member in party.Entities)
        {
            var prefab = Resources.Load<GameObject>("Pieces/"+ member.entityId);
            if(prefab == null)
            {
                Debug.Log("NO ENTITY!!!");
                continue;
            }
            var obj = Instantiate(prefab);
            Entity entity = obj.GetComponent<Entity>();
            entities.Add(entity);
            entity.teamNum = num;
            entity.isReflect = isReflect;
            entity.OnDestroyed += (entity) =>
            {
                entities.Remove(entity);
            };
            var reflectVariable = isReflect ? -1 : 1;
            entity.transform.position = new Vector3(-45*reflectVariable, 0, x*reflectVariable);
            x += 10;
        }
    }
    

    public Dictionary<Entity,Tile> OperActivate()
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
                //해당 기물이 이동가능한 칸이 있는지 확인
                var area = tempEntity.GetEntityArea();
                bool movable = false;
                while (true)
                {
                    if (area.Count <= 0) break;
                    var tempTile = area[Random.Range(0, area.Count)];
                    if (currentField.IsMovable(tempTile))
                    {
                        movable = true;
                        selectedTile = currentField.GetTile(tempTile);
                        Debug.Log("POS : " + tempTile.x + tempTile.y);
                        break;
                    }
                    else
                    {
                        area.Remove(tempTile);
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
        var result = new Dictionary<Entity, Tile>();
        if (selectedEntity != null && selectedTile != null)
        {
            result.Add(selectedEntity, selectedTile);
        }
        selectedTile = null;
        selectedEntity = null;
        return result;
    }
    #region EntitySelectInput
    //엔티티 선택
    public void EntitySelect(Entity entity)
    {
        if (!entities.Contains(entity)) return;
        selectedEntity = entity;
    }
    public void EntityMoveUp()
    {
        Debug.Log(selectedEntity.curTile);
        selectedEntity.transform.position = selectedEntity.curTile.transform.position;
        if (selectedTile == selectedEntity.curTile)
        {
            selectedEntity = null;
            selectedTile = null;
        }
    }
    //이동 실행
    public void EntitySetUp(Entity entity)
    {
        //팀이 아니면 이동 권한 없음
        if (!entities.Contains(entity)) return;
        if (selectedTile != null)
        {
            if (selectedTile == entity.curTile)
            {
                Debug.Log("Not Move");
            }
            entity.MoveToTile(selectedTile);
            selectedTile = null;
            selectedEntity = null;
        }
    }
    void EntityDrag(Entity entity)
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
    }
    public void EntitySetDrag(Entity entity)
    {
        EntityDrag(entity);
        //가장 가까운 타일 선택
        List<Tile> movableTile = GameManager.Instance.field.GetHalfTiles(isReflect);
        //범위 내 타일 중에 이동 가능한 가장 가까운 타일을 가져온다.
        Tile closestTile = GetClosestTile(entity.transform.position, movableTile);
        if (closestTile != null) selectedTile = closestTile;
    }
    public void EntityMoveDrag(Entity entity)
    {
        EntityDrag(entity);
        List<Tile> movableTile = new List<Tile>();
        movableTile.Add(entity.curTile);
        foreach (var tilePos in entity.GetEntityArea())
        {
            var tile = currentField.GetTile(tilePos);
            if (tile != null) movableTile.Add(tile);
        }
        Tile closestTile = GetClosestTile(entity.transform.position, movableTile);
        if (closestTile != null) selectedTile = closestTile;
    }

    public Tile GetClosestTile(Vector3 pos, List<Tile> tiles)
    {
        if (tiles == null) return null;
        float minDistance = 0;
        Tile closestTile = null;
        foreach (Tile tile in tiles)
        {
            if (tile.isOccupied && tile.GetEntity() != selectedEntity) continue;
            var distance = (tile.transform.position - pos).magnitude;
            if (closestTile == null || minDistance > distance)
            {
                minDistance = distance;
                closestTile = tile;
            }
        }
        return closestTile;
    }
    #endregion
}