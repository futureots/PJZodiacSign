using System;
using System.Collections.Generic;


namespace PlayerInput
{
    public class RepairModeInput : MoveModeInput
    {
        public Action<Entity, Entity> OnEnhanceRequested;
        public RepairModeInput(InputManager input) : base(input)
        {
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
                        if (target != _selectedEntity && _selectedEntity.baseData == target.baseData && _selectedEntity.Level == target.Level && target.team.IsAlly(_selectedEntity.team))
                        {
                            // 강화 확인 다이얼로그 표시
                            
                            OnEnhanceRequested?.Invoke(target, _selectedEntity);
                            return;
                        }
                    }
                }
            }
            else if (!_selectedEntity.CurTile.Equals(_selectedTile))
            {
                //커맨드 생성
                _inputManager.agent.CreateMoveCommand(_selectedEntity, _selectedTile, true);
            }
        }

        
    }
}
