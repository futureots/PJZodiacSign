using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

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
        public string EffectName { get; private set; } = "";
        public string Description { get; private set; }= "";
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

        #region localization
        
        [SerializeField] private LocalizedString augmentCombinedData;

        // ScriptableObject가 로드될 때 이벤트 구독
        private void OnEnable()
        {
            // 언어가 변경되거나 텍스트가 처음 로드될 때 자동으로 OnStringChanged 함수를 실행하도록 연결(당장 비어있어도 게임 실행 시 세팅 됨)
            augmentCombinedData.StringChanged += OnStringChanged;
        }

        // ScriptableObject가 언로드될 때 이벤트 해제 (메모리 누수 방지)
        private void OnDisable()
        {
            augmentCombinedData.StringChanged -= OnStringChanged;
        }

        // 언어가 바뀌거나 최초 로드될 때 딱 한 번만 실행되는 함수
        private void OnStringChanged(string newText)
        {
            if (string.IsNullOrEmpty(newText)) return;

            // '|' 문자를 기준으로 앞(이름)과 뒤(설명)를 분리해서 캐시에 저장
            string[] splitText = newText.Split('|');
            if (splitText.Length >= 2)
            {
                EffectName = splitText[0].Trim();
                Description = splitText[1].Trim();
            }
            else
            {
                EffectName = newText;
                Description = string.Empty;
            }
        }

        #endregion
        
    }
}
