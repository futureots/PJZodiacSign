

public class EntityGoodsUI : GoodsUI<EntityData>
{
    public override void SetGoods(EntityData data)
    {
        buyBtn.onClick.RemoveAllListeners();
        base.SetGoods(data);
        buyBtn.onClick.AddListener(() =>
        {
            var customer = Agent.LocalPlayer;
            var teamNum = customer.teamNum;
            var tiles = Field.GetEmptyTiles(StageManager.Instance.agentField[teamNum].GetTiles());
            if(tiles.Count > 0)
            {
                var entity = EntityFactory.RequestEntity(data, new intVector2(1, 1), tiles[0]);
                entity.team.teamNumber = teamNum;
                customer.Credit -= price;
            }
            else
            {
                EditorLogger.Print("소환할 빈 공간이 없습니다!");
            }
        });
    }
}
