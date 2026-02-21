using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour , IInput
{
    [SerializeField] Agent agent;

    public TurnType curTurnType { get; private set; }

    private void Start()
    {
    }

    public void Init(Agent agent)
    {
        this.agent = agent;
        agent.fieldController.OnTurnStarted += OnTurnChange;
    }

    
    public void OnTurnChange(Turn curTurn)
    {
        curTurnType = curTurn.type;
        
        if(curTurn.agentID == agent.id)
        {
            EditorLogger.Print("AI "+ curTurnType.ToString());
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

    public void AttackInput()
    {
        var list = StageManager.Instance.field.GetEntities(agent.id);
        EditorLogger.Print(list.Count);

        foreach (var entity in list)
        {
            agent.CreateAttackCommand(entity);
        }
        agent.CreateEndCommand();
    }

    public IEnumerator SetRepairMode()
    {
        // 보유한 크레딧으로 상점을 통해 기물 구매
        var emptyTiles = Field.GetEmptyTiles(StageManager.Instance.agentField[agent.id].GetTiles());
        foreach (var emptyTile in emptyTiles)
        {
            var data = StageManager.Instance.shop.GetRandomEntity(agent.Credit);
            if (!data) break;
            agent.Credit -= data.normalPrice;
            var entity = EntityFactory.RequestEntity(data, new intVector2(-1, -1), emptyTile);
        }

        // 남은 크레딧으로 기물 강화
        foreach (var entity in StageManager.Instance.agentField[agent.id].GetEntities())
        {
            var mul = 1;
            for (int i = 0; i < entity.Level; i++) mul *= 2;
            if (mul * entity.baseData.normalPrice < agent.Credit)
            {
                agent.Credit -= mul * entity.baseData.normalPrice;
                entity.Level += 1;
            }
        }
        
        yield return null;
        // 내 필드에 있는 기물을 메인 필드에 배치
        var fieldTiles = Field.GetEmptyTiles(StageManager.Instance.field.GetHalfTiles(true));
        foreach (var entity in StageManager.Instance.agentField[agent.id].GetEntities())
        {
            // 빈 타일 중 랜덤 위치 선택
            Tile tile = fieldTiles[UnityEngine.Random.Range(0, fieldTiles.Count)];

            // 선택한 위치에 기물 이동
            agent.CreateMoveCommand(entity, tile,true);
            fieldTiles.Remove(tile);
        }

        agent.CreateEndCommand();
        EditorLogger.Print("RepairEnd");
    }

    public IEnumerator SetActionMode()
    {
        yield return new WaitForSeconds(0.5f);
        var flag = false;
        Action<int> wait = i =>
        {
            if (i == 0) flag = true;
            else flag = false;
        };
        StageManager.Instance.isSequencing += wait;
        

        for (int i = 0; i < agent.actionCount;i++) {
            // TODO : 스킬을 사용할 수 있으면 스킬을 사용한다.
            int max = -9999;
            List<KeyValuePair<Entity, intVector2>> bestAct = new();
            foreach (var checkEntity in agent.actionAbleEntities)
            {
                // 필드 값 가져오기
                int[,] field = StageManager.Instance.field.GetFieldState(checkEntity);

                // 적의 공격범위 가져오기 및 예상 데미지 계산
                var values = StageManager.Instance.field.CalculateEnemyThreat(field, agent.id);

                // 가장 좋은 위치의 행동 가져오기
                if (TryGetBestMove(checkEntity, field, values, out int value, out intVector2 pos))
                {
                    // 같은 값일 경우 리스트에 추가해서 랜덤 추출
                    if (max < value)
                    {
                        EditorLogger.Print($"Best Entity : {checkEntity.name} , BestPos : {pos} , Value : {value}");
                        bestAct.Clear();
                        max = value;
                        bestAct.Add(new KeyValuePair<Entity, intVector2>(checkEntity, pos));
                    }
                    else if (max == value)
                    {
                        bestAct.Add(new KeyValuePair<Entity, intVector2>(checkEntity, pos));
                    }
                }
            }

            if (bestAct.Count > 0)
            {
                var best = bestAct[Random.Range(0, bestAct.Count)];
                agent.CreateMoveCommand(best.Key, StageManager.Instance.field.GetTile(best.Value));
                agent.actionAbleEntities.Remove(best.Key);
                yield return new WaitUntil(() => flag);
            }
        }
        StageManager.Instance.isSequencing -= wait;
        agent.CreateEndCommand();
    }



    bool TryGetBestMove(
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
        // TODO : 현재 타일의 이득값을 계산
        var plusArea = area.GetAttackVector(field, pos, entity.direction);
        field[pos.y, pos.x] = (int)agent.id;
        foreach (var plus in plusArea)
        {
            if (field[plus.y, plus.x] == Field.EmptyTileIndex) continue;
            if (field[plus.y, plus.x] == (int)agent.id) continue;
            tileValues[pos.y, pos.x] += power + 1;
        } 
        value = tileValues[entity.CurTile.fieldPos.y, entity.CurTile.fieldPos.x];
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

    public IEnumerator InputEntity(List<Entity> list, Action<Entity> input, Action<bool> callback, int count = -1)
    {
        var inputCount = Mathf.Min(list.Count, count);
        for (int i = 0; i < inputCount; i++)
        {
            input?.Invoke(list[i]);
        }
        callback?.Invoke(true);
        yield break;
    }

    public IEnumerator InputTile(List<Tile> list, Action<Tile> input, Action<bool> callback, int count = -1)
    {
        var inputCount = Mathf.Min(list.Count, count);
        for (int i = 0; i < inputCount; i++)
        {
            input?.Invoke(list[i]);
        }
        callback?.Invoke(true);
        yield break;
    }
}


