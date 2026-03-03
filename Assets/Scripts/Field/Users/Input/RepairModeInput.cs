using System;
using System.Collections.Generic;


namespace PlayerInput
{
    public class RepairModeInput : MoveModeInput
    {
        public Action<Entity, Entity> onEnhanceRequested;
        public RepairModeInput(InputManager input) : base(input)
        {
        }
        
        protected override List<Tile> GetMovableTiles()
        {
            // 배치가 가능한 타일 리스트(리소스 필드 + 메인 필드에 배치 가능한 공간)
            var movableTiles = new List<Tile>();
            movableTiles.AddRange(StageManager.Instance.agentField[inputManager.agent.id].GetTiles());
            movableTiles.AddRange(StageManager.Instance.field.GetHalfTiles());
            return movableTiles;
        }

        protected override void DragAction()
        {
            // 해당 타일로 이동
            if (!selectedTile.IsEmpty)
            {
                if(selectedTile.occupiedEntity.TryGetComponent<Entity>(out var target))
                {
                    if (target != null)
                    {
                        // 같은 기물이 같은 레벨이면 강화
                        if (target != selectedEntity && selectedEntity.baseData == target.baseData && selectedEntity.Level == target.Level && target.team.IsAlly(selectedEntity.team))
                        {
                            // 강화 확인 다이얼로그 표시
                            
                            onEnhanceRequested?.Invoke(target, selectedEntity);
                            return;
                        }
                    }
                }
            }
            else if (!selectedEntity.CurTile.Equals(selectedTile))
            {
                // 기물이 리소스에서 메인 필드로 이동하는 경우
                if (!selectedEntity.CurTile.field.Equals(selectedTile.field) && selectedTile.field.Equals(StageManager.Instance.field))
                {
                    var fieldEntities = StageManager.Instance.field.GetEntities(inputManager.agent.id);
                    // 필드에 배치가 가능할 경우
                    if (inputManager.MaxEntityCount > fieldEntities.Count)
                    {
                        //커맨드 생성
                        inputManager.agent.CreateMoveCommand(selectedEntity, selectedTile, true);
                        // TODO : 배치 가능 수를 넘어섰다는 메세지 표시(전송)
                    }
                }
                else
                {
                    //커맨드 생성
                    inputManager.agent.CreateMoveCommand(selectedEntity, selectedTile, true);
                }
                
            }
        }

        
    }
}
