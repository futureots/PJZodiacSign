using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    /*
     * 스테이지 레벨 1개 생성 관리자
     * - 레벨 로드시 데이터를 GameManager로부터 전달받아 레벨 생성
     * - Controller씬 로드
     * - 필요 데이터 전달, 의존성 주입
     * - FieldController Init 수행
     */

    public int level {  get; private set; }
    public bool loadFinished = false;
    
    public HpPanelManager hpManager;
    public TeamColorTable teamColorTable;

    public void Init( /*LevelData data*/ string levelData)
    {
        loadFinished = false;
        
        // TODO: LevelData 확인 후 씬 로드
        SceneManager.LoadScene(levelData);
        // TODO: 데이터 전달
    }

    public void Load()
    {
        /* TODO: 컨트롤러에 따라 필드 로드
         * - 
        */ 
    }
}
