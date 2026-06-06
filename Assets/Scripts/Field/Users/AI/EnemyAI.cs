using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour , IInput
{
    protected Agent agent;

    protected TurnType CurTurnType { get; private set; }
    

    public void Init(Agent agent)
    {
        this.agent = agent;
        agent.fieldController.OnTurnStarted += OnTurnChange;
    }


    private void OnTurnChange(Turn curTurn, uint count)
    {
        CurTurnType = curTurn.type;
        
        if(curTurn.agentID == agent.id)
        {
            switch (curTurn.type)
            {
                case TurnType.ACTION:
                    StartCoroutine(SetActionMode());
                    break;
                case TurnType.ATTACK:
                    AttackInput();
                    break;
                case TurnType.REPAIR:
                    StartCoroutine(SetRepairMode());
                    break;
            }
        }
    }

    private void AttackInput()
    {
        var list = StageManager.Instance.field.GetEntities(agent.id);

        foreach (var entity in list)
        {
            agent.CreateAttackCommand(entity);
        }
        agent.CreateEndCommand();
    }

    protected virtual IEnumerator SetRepairMode()
    {
        // 보유한 크레딧으로 상점을 통해 기물 구매(8~32개 기물 구매)
        List<Tile> emptyTiles = StageManager.Instance.agentField[agent.id].GetTiles().GetEmptyTiles();
        int count = Random.Range(Math.Min(8,emptyTiles.Count),emptyTiles.Count);
        for(int i=0;i<count;i++)
        {
            if (!BuyEntity(emptyTiles[i]))
            {
                break;
            }
        }

        yield return null;

        // 남은 크레딧으로 기물 강화(최대 50회 강화)
        for(int i=0;i<50;i++)
        {
            var entities = StageManager.Instance.agentField[agent.id].GetEntities().FindAll(e=> e.baseData.normalPrice*Math.Pow(2,e.Level)<=agent.Credit);
            if (entities.Count <= 0) break;
            
            var entity = entities[Random.Range(0, entities.Count)];
            EnhanceEntity(entity);
        }
        
        
        yield return null;
        
        // 내 필드에 있는 기물을 메인 필드에 배치
        var fieldTiles = StageManager.Instance.field.GetHalfTiles(true).GetEmptyTiles();
        foreach (var entity in StageManager.Instance.agentField[agent.id].GetEntities())
        {
            // 빈 타일 중 랜덤 위치 선택
            Tile tile = fieldTiles[Random.Range(0, fieldTiles.Count)];

            // 선택한 위치에 기물 이동
            agent.CreateMoveCommand(entity, tile,true);
            fieldTiles.Remove(tile);
        }

        agent.CreateEndCommand();
    }

    /// <summary>
    /// 구매가능한 기물 1개 구매
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    protected bool BuyEntity(Tile tile)
    {
        var data = StageManager.Instance.shop.GetRandomEntity(agent.Credit,ShopTable.ShopType.Normal);
        if (!data) return false;
        agent.Credit -= data.normalPrice;
        var entity = EntityFactory.Instance.Request(data, new intVector2(-1, -1), tile, agent.id);
        return true;
    }

    /// <summary>
    /// 강화 가능한 기물 1회 강화
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected void EnhanceEntity(Entity entity)
    {
        var mul = 1;
        for (int i = 0; i < entity.Level; i++) mul *= 2;
        if (mul * entity.baseData.normalPrice < agent.Credit)
        {
            agent.Credit -= mul * entity.baseData.normalPrice;
            entity.Level += 1;
        }
    }

    protected IEnumerator SetActionMode()
    {
        yield return new WaitForSeconds(0.5f);
        var flag = true;
        Action<int> wait = i =>
        {
            if (i == 0) flag = true;
            else flag = false;
        };
        StageManager.Instance.isSequencing += wait;
        
        for (int i = 0; i < agent.actionCount;i++)
        {
            yield return EnemyAction().ToCoroutine();
            yield return new WaitUntil(() => flag);
        }
        StageManager.Instance.isSequencing -= wait;
        agent.CreateEndCommand();
    }

    protected async UniTask<bool> EnemyMoveAction(Entity entity)
    {
        // 해당 기물을 가치가 가장 높은 위치로 이동

        if (!entity.IsControllable) return false;
        // 필드 값 가져오기
        int[,] field = StageManager.Instance.field.GetFieldState(entity);

        // 적의 공격범위 가져오기 및 예상 데미지 계산
        var values = StageManager.Instance.field.CalculateEnemyThreat(field, agent.id);

        // 가장 좋은 위치의 행동 가져오기
        if (TryGetBestMove(entity, field, values, out int value, out intVector2 pos))
        {
            if (entity.CurTile.fieldPos == pos)
            {
                return false;
            }
            else
            {
                agent.CreateMoveCommand(entity, StageManager.Instance.field.GetTile(pos));
                entity.IsControllable = false;
                return true;
            }
        }
        
        // 좋은 행동이 없을 경우 다른 기물 찾아보기
        return false;
    }

    protected async UniTask<bool> EnemySkillAction(Entity entity)
    {
        if (!entity.energy.IsFull()) return false;

        if (!entity.skill.skillLogic.IsValuable()) return false;
        
        // 스킬 입력 시도(실패 시 실제 입력X)
        var result =  await entity.skill.skillLogic.InputSkill(this);
        if (result)
        {
            // 스킬 실행
            agent.CreateSkillCommand(entity.skill);
            return true;
        }

        return false;
    }
    protected virtual async UniTask EnemyAction()
    {
        // 랜덤 기물을 선택, 해당 기물의 스킬사용이 가능한지 확인, 되면 실행, 안되면 이동가능한지 확인, 되면 실행 안되면 해당 기물 빼고 리트라이
        var list = new List<Entity>(agent.fieldEntities);
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (list.Count <= 0) break;
            int rand = Random.Range(0, list.Count);
            var selectEntity = list[rand];
            // 선택한 기물 스킬 사용 시도
            var result = await EnemySkillAction(selectEntity);
            // 스킬 사용 성공 시 종료
            if (result) return;
            // 기물 이동 시도
            result = await EnemyMoveAction(selectEntity);
            // 이동 성공 시 종료
            if (result) return;
            // 행동할 불가 기물 제거 후 재시도
            list.RemoveAt(rand);
        }
    }

    
    protected bool TryGetBestMove(
        Entity entity,
        int[,] field,
        int[,] tileValues,
        out int value,
        out intVector2 pos)
    {
        // 이동할 가치가 있는지 판단
        bool isWorthy = false;

        int power = entity.Power;
        var area = entity.area;
        
        pos = entity.CurTile.fieldPos;
        // 현재 타일의 이득값을 계산
        var plusArea = area.GetAttackVector(field, pos, entity.direction);
        field[pos.y, pos.x] = (int)agent.id;
        foreach (var plus in plusArea)
        {
            if (field[plus.y, plus.x] == Field.EmptyTileIndex) continue;
            if (field[plus.y, plus.x] == (int)agent.id) continue;
            tileValues[pos.y, pos.x] += power + 1;
        } 
        value = tileValues[entity.CurTile.fieldPos.y, entity.CurTile.fieldPos.x] - 1;
        List<intVector2> valuablePos = new();
        
        var moveVectors = area.GetMoveVector(field, entity.CurTile.fieldPos, entity.direction);
        // TODO : 이동 범위 타일의 이득값을 계산, 현재 타일보다 이득값이 클 경우 갱신
        foreach (var moveVector in moveVectors)
        {
            // 이동할 수 없는 타일은 제외
            if (field[moveVector.y, moveVector.x] != Field.EmptyTileIndex || entity.CurTile.fieldPos == moveVector) continue;

            // 공격 가능 체크
            field[entity.CurTile.fieldPos.y, entity.CurTile.fieldPos.x] = Field.EmptyTileIndex;
            plusArea = area.GetAttackVector(field, moveVector, entity.direction);
            field[entity.CurTile.fieldPos.y, entity.CurTile.fieldPos.x] = (int)agent.id;
            foreach (var plus in plusArea)
            {
                if (field[plus.y, plus.x] == Field.EmptyTileIndex) continue;
                if (field[plus.y, plus.x] == (int)agent.id) continue;
                tileValues[moveVector.y, moveVector.x] += power + 1;
            }


            if (value < tileValues[moveVector.y, moveVector.x])
            {
                valuablePos.Clear();
                value = tileValues[moveVector.y, moveVector.x];
                isWorthy = true;
                valuablePos.Add(moveVector);
            }
            else if (value == tileValues[moveVector.y, moveVector.x] && isWorthy)
            {
                valuablePos.Add(moveVector);
            }
        }

        if (isWorthy)
        {
            pos = valuablePos[Random.Range(0, valuablePos.Count)];
            return true;
        }
        return false;
    }
    

    public UniTask<List<Entity>> InputEntity(List<Entity> list, int count = -1)
    {
        if (list.Count <= 0) return new UniTask<List<Entity>>(null);
        if(list.Count <= count) return new UniTask<List<Entity>>(list);
        List<Entity> result = new();
        for (int i = 0; i < count; i++)
        {
            result.Add(list[i]);
        }

        return UniTask.FromResult(result);
    }

    public UniTask<List<Tile>> InputTile(List<Tile> list, int count = -1)
    {
        if (list.Count <= 0) return new UniTask<List<Tile>>(null);
        if(list.Count <= count) return new UniTask<List<Tile>>(list);
        List<Tile> result = new();
        for (int i = 0; i < count; i++)
        {
            result.Add(list[i]);
        }

        return UniTask.FromResult(result);
    }
}


