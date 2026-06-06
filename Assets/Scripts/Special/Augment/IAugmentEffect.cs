namespace Augment
{
    public interface IAugmentEffect
    {
        /**
         * Augment Effect Interface
         * - 실행 타이밍 별 동작 함수 정의
         */
        AugmentType Type { get; }

        // 획득 시
        void OnActive();

        // 레벨 시작 시
        void OnLevelStart(StageData data);
        
        // 페이즈 시작 시
        void OnPhaseStart(Phase data);
        
        // 턴 시작 시
        void OnTurnStart(Turn data);
    }
}
