

public class EntityGoodsUI : GoodsUI<EntityData>
{
    public override void SetGoods(EntityData data)
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
                var entity = EntityFactory.Instance.Request(data, new intVector2(1, 1), tiles[0]);
                entity.team.teamNumber = teamNum;
                
                customer.Credit -= price;
                entity.isControllable = true;
            }
            else
            {
                EditorLogger.Print("소환할 빈 공간이 없습니다!");
            }
        });
    }
}
