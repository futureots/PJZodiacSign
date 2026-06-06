using System.Collections.Generic;
using UnityEngine;

namespace Augment
{
    public class AugmentManager : ExSystem
    {
        private FieldController _controller;
        private StageData _stageData;
        
        [Header("Augments")]
        [SerializeField] private List<AugmentSO> augmentPool;
        private List<IAugmentEffect> CurrentAugments => _stageData.Augments;
        
        public override void Init(FieldController controller, StageData stageData)
        {
            _controller = controller;
            _stageData = stageData;
            
            // 증강 초기화 설정 (첫 진입)
            _stageData.Augments ??= new List<IAugmentEffect>();
            
            // 증강 추가 액션 연결
            if (augmentPool.Count > 0)
            {
                controller.OnLevelStart += _ =>
                {
                    StartChoice();
                };
            }
            
            // 기존 증강 등록
            foreach (var augment in CurrentAugments)
            {
                RegisterAugment(augment);
            }
        }

        private void StartChoice()
        {
            // 보유 증강 제외
            List<AugmentSO> choicePool = augmentPool.FindAll(augment => !CurrentAugments.Contains(augment));
            
            // 선택 UI 추가 및 결정 로직 추가
            // TODO: 현재는 첫 증강 즉시 적용
            Add(choicePool[0]);
        }

        private void Add(AugmentSO augment)
        {
            // Augment 등록
            RegisterAugment(augment);
            CurrentAugments.Add(augment);
            
            // Augment 획득 이벤트 동작
            augment.OnActive();
        }

        private void RegisterAugment(IAugmentEffect augment)
        {
            _controller.OnLevelStart += augment.OnLevelStart;
            _controller.OnPhaseStart += augment.OnPhaseStart;
            _controller.OnTurnStarted += augment.OnTurnStart;
        }
    }
}