using System.Collections.Generic;
using UnityEngine;

namespace Augment
{
    public enum AugmentType
    {
        Neutral = 0,
        Positive,
        Negative,
    }
    
    public abstract class AugmentSO : ScriptableObject, IAugmentEffect
    {
        // Augment 공용 색상
        public static Dictionary<AugmentType, Color> Color = new()
        {
            [AugmentType.Neutral] = new Color(254, 254, 254),
            [AugmentType.Positive] = new Color(33, 140, 33),
            [AugmentType.Negative] = new Color(254, 63, 63)
        };
        
        // Augment Data
        public Sprite icon;
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
