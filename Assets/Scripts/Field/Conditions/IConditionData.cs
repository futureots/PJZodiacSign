using System;
using System.Collections.Generic;
using UnityEngine;


namespace Condition
{

    public interface IConditionData
    {
        public ConditionType conditionType {  get; }

        public ConditionArgs QueryConditions(ConditionArgs args);
    }

    public enum ConditionType
    {
        TILE,
        ENTITY
    }
    public struct ConditionArgs
    {

        public ConditionArgs(ConditionType type)
        {
            currentType = type;
            entities = null;
            tiles = null;
        }
        public ConditionType currentType;

        public List<Entity> entities;
        public List<Tile> tiles;
        // 이후 다른 입력값 추가 시 리스트 추가하기
    }
}