

using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    /*
     * 스테이지 레벨 1개 관리
     * - 필요 데이터 전달, 의존성 주입
     * - FieldController Init 수행
     */
    [SerializeField] private Shop shop;
    [SerializeField] private EntityFactory entityFactory;

    public void Init( StageData stageData)
    {
        // shop.Init(stageData.shopTable);
        // 
    }

    public void Load()
    {
        /* TODO: 컨트롤러에 따라 필드 로드
         * - 
        */ 
    }
}
