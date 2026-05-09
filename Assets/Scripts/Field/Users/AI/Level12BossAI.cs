using Cysharp.Threading.Tasks;

public class Level12BossAI : EnemyAI
{
    
    protected override async UniTask EnemyAction()
    {
        // 랜덤 기물을 선택, 해당 기물의 스킬사용이 가능한지 확인, 되면 실행, 안되면 이동가능한지 확인, 되면 실행 안되면 해당 기물 빼고 리트라이
        var entity = agent.fieldEntities.Find(e => e.baseData.id == "RuinedKing");
        if (entity)
        {
            // 선택한 기물 스킬 사용 시도
            var result = await EnemySkillAction(entity);
            // 스킬 사용 성공 시 종료
            if (result) return;
        }
        
        await base.EnemyAction();
    }
}
