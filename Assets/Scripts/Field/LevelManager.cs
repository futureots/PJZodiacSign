using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /*
     * 레벨 생성 관리자
     * - 레벨 로드시 데이터를 GameManager로부터 전달받아 레벨 생성
     * - Controller씬 로드
     * - 필요 데이터 전달, 의존성 주입
     * - FieldController Init 수행
     */

    public bool loadFinished = false;

    public void Init( /*LevelData data*/)
    {
        loadFinished = false;
        
        
    }

    private void Load()
    {
        
    }
}
