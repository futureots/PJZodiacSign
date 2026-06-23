using System;
using System.Collections;
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
        private List<AugmentSO> CurrentCurrentAugment => _stageData.currentAugment;
        private List<AugmentSO> ignorePool
        {
            get
            {
                List<AugmentSO> temp = new(CurrentCurrentAugment);
                temp.AddRange(_stageData.unselectedAugment);
                return temp;
            }
        }

        public event Action<List<AugmentSO>> OnAugmentChanged;
        
        public override IEnumerator Init(FieldController controller, StageData stageData)
        {
            _controller = controller;
            _stageData = stageData;
            
            // 증강 초기화 설정 (첫 진입)
            _stageData.currentAugment ??= new List<AugmentSO>();
            
            // 증강 추가 액션 연결
            if (augmentPool.Count > 0)
            {
                controller.OnLevelStarted += _ =>
                {
                    StartChoice();
                };
            }
            
            // 기존 증강 등록
            foreach (var augment in CurrentCurrentAugment)
            {
                RegisterAugment(augment);
            }
            
            OnAugmentChanged?.Invoke(CurrentCurrentAugment);

            // TODO: 증강 선택 코루틴 반환
            return null;
        }

        /// <summary>
        /// 추가할 증강 선택
        /// </summary>
        private void StartChoice()
        {
            // 보유 증강 제외
            List<AugmentSO> choicePool = augmentPool.FindAll(augment => !ignorePool.Contains(augment));
            
            // 선택 UI 추가 및 결정 로직 추가
            // TODO: 현재는 첫 증강 즉시 적용
            Add(choicePool[0]);
        }

        /// <summary>
        /// 증강 등록 및 실행
        /// </summary>
        /// <param name="augment"></param>
        private void Add(AugmentSO augment)
        {
            // Augment 등록
            RegisterAugment(augment);
            CurrentCurrentAugment.Add(augment);
            
            OnAugmentChanged?.Invoke(CurrentCurrentAugment);
            
            // Augment 획득 이벤트 동작
            augment.OnActive();
        }

        // 증강 이벤트 연결
        private void RegisterAugment(IAugmentEffect augment)
        {
            _controller.OnLevelStarted += augment.OnLevelStarted;
            _controller.OnPhaseStarted += augment.OnPhaseStarted;
            _controller.OnTurnStarted += augment.OnTurnStarted;
        }
    }
}