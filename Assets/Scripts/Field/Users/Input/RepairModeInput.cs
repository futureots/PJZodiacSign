using System.Collections.Generic;


namespace PlayerInput
{
    public class RepairModeInput : MoveModeInput
    {

        public RepairModeInput(InputManager input) : base(input)
        {
        }
        public override void SetMode()
        {
            base.SetMode();

        }
        protected override List<Tile> GetMovableTiles()
        {
            // 배치가 가능한 타일 리스트(리소스 필드 + 메인 필드에 배치 가능한 공간)
            var movableTiles = new List<Tile>();
            movableTiles.AddRange(StageManager.Instance.agentField[_inputManager.agent.id].GetTiles());
            movableTiles.AddRange(StageManager.Instance.field.GetHalfTiles());
            return movableTiles;
        }

        protected override void DragAction()
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
                var cmd = new MoveCommand(_selectedEntity, _selectedTile, _selectedEntity.CurTile);
                _inputManager.agent.CreateMoveCommand(_selectedEntity, _selectedTile, true);
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
