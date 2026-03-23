using System.Collections.Generic;
using UnityEngine;

namespace PlayerInput
{
    public class MoveModeInput : IInputState
    {

        protected readonly InputManager inputManager;

        //선택한 엔티티 저장
        protected Entity selectedEntity;
        protected Tile selectedTile;

        // Visualizing
        protected List<Tile> moveArea;
        protected List<Tile> attackArea;

        protected GameObject targetSelector;
        protected GameObject targetTileSelector;
        
        public MoveModeInput(InputManager input)
        {
            inputManager = input;

            moveArea = new List<Tile>();
            attackArea = new List<Tile>();
        }

        public void RemoveMode()
        {
            inputManager.OnObjectClicked.RemoveListener(DragStart);
            Object.Destroy(targetSelector);
            Object.Destroy(targetTileSelector);
            
            inputManager.agent.actionAbleEntities.ForEach(x=>x.isControllable = false);
        }

        public virtual void SetMode()
        {
            inputManager.OnObjectClicked.AddListener(DragStart);
            
            targetSelector = Object.Instantiate(inputManager.entitySelecter);
            targetTileSelector = Object.Instantiate(inputManager.tileSelecter);
            targetSelector.SetActive(false);
            targetTileSelector.SetActive(false);
            if (inputManager.agent.CurrentActionCount == 0)
            {
                inputManager.agent.CreateEndCommand();
            }
            // 이동 가능한 기물을 설정
            inputManager.agent.actionAbleEntities.ForEach(x=>x.isControllable = true);
        }
        
        // 드래그 시작
        protected virtual void DragStart(GameObject obj)
        {
            
            if (!obj) return;

            if (!obj.TryGetComponent(out Entity entity))
            {
                return;
            }

            // 기물이 이동 가능한지 확인
            if (!entity.team.IsAlly(inputManager.agent.id)) return;
            if (!entity.isControllable) return;
            selectedEntity = entity;
            targetSelector.SetActive(true);
            targetSelector.transform.position = selectedEntity.transform.position + Vector3.up * 0.1f;

            selectedTile = entity.CurTile;
            targetTileSelector.SetActive(true);
            targetTileSelector.transform.position = selectedTile.transform.position + Vector3.up * 0.1f;



            // 기물 이동범위 표시
            moveArea = GetMovableTiles(); // 수리 모드일때는 다른 방식으로 가져옴
            foreach (Tile move in moveArea)
            {
                move.ApplyHighlight(Tile.HighLightType.Move);
            }

            attackArea = selectedEntity.GetAttackArea(selectedTile);
            foreach (Tile area in attackArea)
            {
                area.ApplyHighlight(Tile.HighLightType.Attack);
            }


            inputManager.OnMouseMove.AddListener(DragEntity);
            inputManager.OnMouseUp.AddListener(DragEnd);
        }

        // 드래그 중
        protected virtual void DragEntity(Vector2 value)
        {
            Ray ray = Camera.main.ScreenPointToRay(value);
            Plane plane = new Plane(Vector3.up, new Vector3(0, 5, 0));
            Vector3 pos = selectedEntity.transform.position;
            if (plane.Raycast(ray, out float rayDistance))
            {
                pos = ray.GetPoint(rayDistance);
            }
            // 공격 범위 표시
            selectedTile = inputManager.GetClosestTile(pos, moveArea);
            targetTileSelector.transform.position = selectedTile.transform.position + Vector3.up * 0.1f;

            foreach (Tile area in attackArea)
            {
                area.RemoveHighlight(Tile.HighLightType.Attack);
            }
            attackArea = selectedEntity.GetAttackArea(selectedTile);
            foreach (Tile area in attackArea)
            {
                area.ApplyHighlight(Tile.HighLightType.Attack);
            }
        }

        // 드래그 종료
        protected virtual void DragEnd()
        {
            inputManager.OnMouseMove.RemoveListener(DragEntity);
            inputManager.OnMouseUp.RemoveListener(DragEnd);

            // 엔티티 클리어
            if (selectedEntity == null)
            {
                return;
            }

            // 가장 가까운 타일(선택한 타일)
            foreach (var area in attackArea)
            {
                area.RemoveHighlight(Tile.HighLightType.Attack);
            }
            foreach (var move in moveArea)
            {
                move.RemoveHighlight(Tile.HighLightType.Move);
            }
            targetSelector.SetActive(false);
            targetTileSelector.SetActive(false);

                
            DragAction();
            selectedEntity = null;
            selectedTile = null;

        }

        /// <summary>
        /// 이동가능한 타일 리스트 반환
        /// </summary>
        /// <returns></returns>
        protected virtual List<Tile> GetMovableTiles()
        {
            return selectedEntity.GetMoveArea();
        }
        
        /// <summary>
        /// 입력된 데이터로 사용할 명령 입력
        /// </summary>
        protected virtual void DragAction()
        {
            // 제자리 이동 불가능
            if (!selectedEntity.CurTile.Equals(selectedTile) && selectedTile)
            {

                // 커맨드 생성
                inputManager.agent.CreateMoveCommand(selectedEntity, selectedTile);
                selectedEntity.isControllable = false;
                inputManager.agent.actionAbleEntities.Remove(selectedEntity);
                
                EditorLogger.Print($"남은 행동력 {inputManager.agent.CurrentActionCount}");
                // 더이상 행동할 수 없으면 자동으로 턴 종료
                if (inputManager.agent.CurrentActionCount == 0)
                {
                    inputManager.agent.CreateEndCommand();
                    inputManager.ClearInputMode();
                }
            }
        }
    }
}