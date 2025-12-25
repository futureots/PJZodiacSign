using System;
using System.Collections.Generic;


namespace Condition
{

    public interface IFilterData
    {
        public ConditionType conditionType {  get; }

        public ConditionArgs FilterConditions(ConditionArgs args, params ConditionArgs[] prevArgs);
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
            switch (type)
            {
                case ConditionType.TILE:
                    tiles = new();
                    break;
                case ConditionType.ENTITY:
                    entities = new();
                    break;
            }

        }
        public ConditionType currentType;

        public List<Entity> entities;
        public List<Tile> tiles;
        // 이후 다른 입력값 추가 시 리스트 추가하기
    }
    [Serializable]
    public struct ConditionData
    {
        public List<FilterData> args;
        public bool isNeedInput;
        public int inputCount;
        // 이건 없으면 마우스에 위치한 오브젝트만 표시
        public FilterData outputArea;
    }
}