namespace Augment
{
    public sealed class NewAugmentEffect : IAugmentEffect
    {
        /**
         * 증강 추가용 증강
         */
        public struct NewAugmentContext
        {
            public int amount;
            public AugmentType type;
        }

        public AugmentType Type => AugmentType.Add;
        
        public void OnLevelStart()
        {
            // TODO: 증강 추가 정보 실행
        }
        
        public void OnActive() { }

        public void OnPhaseStart() { }

        public void OnTurnStart() { }
    }
}