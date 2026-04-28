

using UnityEngine;

public class EntityGoodsUI : GoodsUI<EntityData>
{
    public override void SetGoods(EntityData data, LogSystem logSystem = null)
    {
        buyBtn.onClick.RemoveAllListeners();
        base.SetGoods(data);
        buyBtn.onClick.AddListener(() =>
        {
            var customer = Agent.LocalPlayer;
            var teamNum = customer.id;
            var tiles = StageManager.Instance.agentField[teamNum].GetTiles().GetEmptyTiles();
            if(tiles.Count > 0)
            {
                EditorLogger.Print(teamNum);
                var entity = EntityFactory.Instance.Request(data, new intVector2(1, 1), tiles[0], teamNum);
                
                customer.Credit -= price;
                
                entity.IsControllable = true;
            }
            else
            {
                logSystem?.ShowLog("소환할 빈 공간이 없습니다!",LogType.Error);
                EditorLogger.Print("소환할 빈 공간이 없습니다!");
            }
        });
    }
}
