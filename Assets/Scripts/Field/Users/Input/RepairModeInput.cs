using System;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerInput
{
    public class RepairModeInput : MoveModeInput
    {

        public RepairModeInput(InputManager input) : base(input)
        {
        }

        public override List<Tile> GetMovableTiles()
        {
            // 배치가 가능한 타일 리스트(리소스 필드 + 메인 필드에 배치 가능한 공간)
            return base.GetMovableTiles();
        }

        public override void DragAction()
        {
            // 해당 타일로 이동
            if (!_selectedEntity.Move(_selectedTile))
            {
                if(_selectedTile.occupiedObject.TryGetComponent<Entity>(out var target))
                {
                    if (target != null)
                    {
                        // 같은 기물이 같은 레벨이면 강화
                        if (target != _selectedEntity && _selectedEntity.baseData == target.baseData && _selectedEntity.Level == target.Level)
                        {
                            // 강화 확인 다이얼로그 표시
                            ShowEnhanceConfirmDialog(target, _selectedEntity);
                            return;
                        }
                    }
                }
            }
            else if (!_selectedEntity.CurTile.Equals(_selectedTile))
            {
                //커맨드 생성
                var cmd = new MoveCommand(_selectedEntity, _selectedTile);
                _inputManager.agent.CreateMoveCommand(_selectedEntity, _selectedTile);
                _inputManager.agent.SendCommand();
            }
        }

        /// <summary>
        /// 강화 확인 다이얼로그를 표시합니다.
        /// </summary>
        /// <param name="target">강화될 대상 기물</param>
        /// <param name="source">강화에 사용될 기물</param>
        private void ShowEnhanceConfirmDialog(Entity target, Entity source)
        {
            // 강화 확인창을 띄우기 전에 모든 이벤트 구독 해제
            _inputManager.OnObjectClicked.RemoveListener(DragStart);
            _inputManager.OnMouseUp.RemoveListener(DragEnd);
            _inputManager.OnMouseMove.RemoveListener(DragEntity);
            
            // 선택자와 영역 표시 제거
            targetSelecter.SetActive(false);
            targetTileSelecter.SetActive(false);

            // 강화 UI 표시, '확인' 선택 시 강화 커맨드 전송, 아니면 아무일도 일어나지 않음
            //_inputManager.turnEndButton.interactable = false;
            //_inputManager.UI.inventory.gameObject.SetActive(false);
            //_inputManager.UI.confirmDialog.ShowDialog(
            //    "강화하시겠습니까?",
            //    (Action)(() => {
            //        // 확인 시 강화 실행
            //        Debug.Log("Enhance Confirmed");
            //        target.Level += 1;
            //        source.CurTile.ClearOccupant();
            //        _selectedEntity = null;

            //        // 강화 완료 후 이벤트 구독 재설정
            //        RestoreEventSubscriptions();
            //        _inputManager.UI.entityInfo.HidePanel();
            //    }),
            //    (Action)(() => {
            //        // 취소 시 원래 위치로 복귀
            //        Debug.Log("Enhance Cancelled");
            //        _selectedEntity.transform.position = _selectedEntity.CurTile.transform.position;
            //        _selectedEntity = null;

            //        // 강화 취소 후 이벤트 구독 재설정
            //        RestoreEventSubscriptions();
            //    })
            //);
        }

        /// <summary>
        /// 이벤트 구독을 재설정합니다.
        /// </summary>
        void RestoreEventSubscriptions()
        {
            _inputManager.OnObjectClicked.AddListener(DragStart);
            _inputManager.OnMouseUp.AddListener(DragEnd);
            //_inputManager.turnEndButton.interactable = true;
            //_inputManager.UI.inventory.gameObject.SetActive(true);
        }
    }
}
