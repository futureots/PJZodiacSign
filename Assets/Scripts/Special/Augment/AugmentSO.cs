using System;
using UnityEngine;

namespace Augment
{
    [Flags]
    public enum AugmentType
    {
        Add = 0,
        Neutral = 1 << 0,
        Positive = 1 << 1,
        Negative = 1 << 2,
    }
    
    public abstract class AugmentSO : ScriptableObject, IAugmentEffect
    {
        // Augment Data
        public string effectName = "";
        public string description = "";
        [SerializeField] private AugmentType type = AugmentType.Neutral;
        public AugmentType Type => type;
        
        /// <summary>
        /// 획득 즉시 실행
        /// </summary>
        public virtual void OnActive() { }

        /// <summary>
        /// 레벨 시작시 실행
        /// </summary>
        /// <param name="data">스테이지 정보</param>
        public virtual void OnLevelStarted(StageData data) { }

        /// <summary>
        /// 페이즈 시작시 실행
        /// </summary>
        /// <param name="data">페이즈 정보</param>
        public virtual void OnPhaseStarted(Phase data) { }

        /// <summary>
        /// 턴 시작시 실행
        /// </summary>
        /// <param name="data">턴 정보</param>
        /// <param name="turnCount">현재 누적 턴 수</param>
        public virtual void OnTurnStarted(Turn data, uint turnCount) { }
    }
}
