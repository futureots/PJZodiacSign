using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour , IInput
{
    Agent agent;

    public TurnType curTurnType { get; private set; }

    private void Start()
    {
        if(TryGetComponent<Agent>(out var agent))
        {
            Init(agent);
        }
    }

    public void Init(Agent agent)
    {
        this.agent = agent;
        EditorLogger.Print(agent.fieldController);
        agent.fieldController.onTurnStarted += OnTurnChanged;
    }

    
    public void OnTurnChanged(Turn curTurn)
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
        foreach (var entity in agent.entities)
        {
            agent.CreateAttackCommand(entity);
        }
        agent.CreateEndCommand();
        agent.SendCommand();
    }

    public IEnumerator SetRepairMode()
    {
        yield return null;
        if (TryGetComponent<SkillComponent>(out var skill))
        {
            agent.CreateSkillCommand(skill);
        }
        // 내 필드에 있는 기물을 메인 필드에 배치
        // var fieldTiles = Field.GetEmptyTiles(GameManager.Instance.field.GetHalfTiles(controller.isReflect));
        // foreach (var entity in controller.resourceEntities)
        // {
        //     // 빈 타일 중 랜덤 위치 선택
        //     Tile tile = fieldTiles[UnityEngine.Random.Range(0, fieldTiles.Count)];
        //     
        //     // 선택한 위치에 기물 이동
        //     entity.Move(tile);
        //     fieldTiles.Remove(tile);
        // }

        agent.CreateEndCommand();
        agent.SendCommand();
        EditorLogger.Print("RepairEnd");
    }

    public IEnumerator SetActionMode()
    {
        yield return new WaitForSeconds(0.5f);
        // TODO : 스킬을 사용할 수 있으면 스킬을 사용한다.

        int max = 0;
        Entity bestEntity = null;
        intVector2 bestPos = new intVector2(-1, -1);
        
        bool flag = false;
        foreach (var checkEntity in agent.entities)
        {

            // 필드 값 가져오기
            int[,] field = StageManager.Instance.field.GetFieldState(checkEntity);

            // 적의 공격범위 가져오기 및 예상 데미지 계산
            var values = StageManager.Instance.field.CalculateEnemyThreat(field, checkEntity.team.teamNumber);

            // 가장 좋은 위치의 행동 가져오기
            if (TryGetBestMove(checkEntity, field, values, out int value, out intVector2 pos))
            {
                flag = true;
                EditorLogger.Print($"Best Entity : {checkEntity.name} , BestPos : {pos} , Value : {value}");
                // 같은 값일 경우 이후의 명령만 가짐
                if (max < value || bestEntity == null)
                {
                    max = value;
                    bestEntity = checkEntity;
                    bestPos = pos;
                }
            }
        }
        if (flag)
        {
            agent.CreateMoveCommand(bestEntity, StageManager.Instance.field.GetTile(bestPos));
        }
        if (TryGetComponent<SkillComponent>(out var skill))
        {
            agent.CreateSkillCommand(skill);
        }
        agent.CreateEndCommand();
        agent.SendCommand();
    }



    bool TryGetBestMove(
        Entity entity,
        int[,] field,
        int[,] tileValues,
        out int value,
        out intVector2 pos)
    {

        int power = 0;
        if (entity.TryGetComponent<PowerComponent>(out var component))
        {
            power = component.Power;
        }
        var tile = entity.CurTile;
        int max = tileValues[tile.fieldPos.y, tile.fieldPos.x];

        List<intVector2> valuablePos = new();
        if (entity.TryGetComponent<AreaComponent>(out var area))
        {
            var list = area.GetMoveVector(field, tile.fieldPos, entity.IsReflect);

            foreach (var item in list)
            {
                // 이동할 수 없는 타일은 제외
                if (field[item.y, item.x] != 0 || tile.fieldPos == item) continue;
                // 죽음 위험 체크(이동 후 체력이 0 이하면 가중치 부여)
                var damage = tileValues[item.y, item.x];
                if (damage + entity.CurHealth <= 0) damage -= 5;

                // 공격 가능 체크
                field[tile.fieldPos.y, tile.fieldPos.x] = 0;
                var plusArea = area.GetAttackVector(field, item, entity.IsReflect);
                field[tile.fieldPos.y, tile.fieldPos.x] = agent.id;
                foreach (var plus in plusArea)
                {
                    if (field[plus.y, plus.x] == 0) continue;
                    if (field[plus.y, plus.x] == agent.id) continue;
                    tileValues[item.y, item.x] += power;
                }

                if (damage > max || valuablePos.Count == 0)
                {
                    valuablePos.Clear();
                    max = damage;
                    valuablePos.Add(item);
                }
                else if (damage == max)
                {
                    valuablePos.Add(item);
                }
            }
        }
        if (valuablePos.Count <= 0)
        {
            value = 0;
            pos = intVector2.Zero;

            return false;
        }
        value = max;
        pos = valuablePos[UnityEngine.Random.Range(0, valuablePos.Count)];
        return true;
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


