using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Augment
{
    public sealed class AugmentManager : ExSystem
    {
        private FieldController _controller;
        private StageData _stageData;

        [Header("Augments")]
        [SerializeField] private Transform selectUI;
        [SerializeField] private List<AugmentSO> augmentPool;
        public int selectCount = 3;
        [SerializeField] private bool onSelecting = false;
        
        private List<AugmentSO> CurrentAugment => DataManager.Instance.GetAugments(DataManager.Instance.playData.currentAugment);
        private List<AugmentSO> IgnorePool => DataManager.Instance.GetAugments(DataManager.Instance.playData.ignoreAugment);

        public event Action<List<AugmentSO>> OnAugmentChanged;
        
        public override IEnumerator Init(FieldController controller, StageData stageData)
        {
            _controller = controller;
            _stageData = stageData;
            
            // 기존 증강 등록
            foreach (var augment in CurrentAugment)
            {
                RegisterAugment(augment);
            }
            
            OnAugmentChanged?.Invoke(CurrentAugment);
            
            bool isAugmentAdd = selectUI && augmentPool.Count > 0;
            return isAugmentAdd ? StartChoice() : null;
        }

        /// <summary>
        /// 추가할 증강 선택
        /// </summary>
        private IEnumerator StartChoice()
        {
            // 보유 증강 제외 선택 풀 생성
            List<AugmentSO> choicePool = augmentPool.FindAll(augment => !IgnorePool.Contains(augment));
            var selectPool = choicePool.GetRandomRange(selectCount);
            
            // 설정 패널 생성
            var contentPanel = selectUI.GetChild(0);
            AugmentInfoUI panelPrefab = contentPanel.GetChild(0).GetComponent<AugmentInfoUI>();
            selectUI.gameObject.SetActive(true);
            onSelecting = true;
            
            // 증강 패널 생성
            foreach (var augment in selectPool)
            {
                var selectAugment = augment;
                AugmentInfoUI newPanel = Instantiate(panelPrefab, contentPanel);
                newPanel.Init(selectAugment);
                newPanel.selectButton.onClick.AddListener(() =>
                {
                    onSelecting = false;
                    Add(selectAugment);
                });
                newPanel.gameObject.SetActive(true);  
                
                // 제외 풀에 추가
                DataManager.Instance.playData.ignoreAugment.Add(augment.Id);
            }
            
            //선택 대기
            yield return new WaitUntil(() => !onSelecting);
            
            // 선택 종료 및 패널 해제
            selectUI.gameObject.SetActive(false);
        }

        /// <summary>
        /// 증강 등록 및 실행
        /// </summary>
        /// <param name="augment"></param>
        private void Add(AugmentSO augment)
        {
            // Augment 등록
            RegisterAugment(augment);
            DataManager.Instance.playData.currentAugment.Add(augment.Id);
            
            OnAugmentChanged?.Invoke(CurrentAugment);
            
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