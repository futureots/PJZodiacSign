using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInput
{
    public class MoveModeInput : IInputState
    {

        protected InputManager _inputManager;

        //선택한 엔티티 저장
        protected Entity _selectedEntity = null;
        protected Tile _selectedTile = null;

        // Visualizing
        protected List<Tile> moveArea;
        protected List<Tile> attackArea;

        protected GameObject targetSelecter = null;
        protected GameObject targetTileSelecter = null;


        public MoveModeInput(InputManager input)
        {
            _inputManager = input;

            moveArea = new List<Tile>();
            attackArea = new List<Tile>();
        }

        public void RemoveMode()
        {
            _inputManager.OnObjectClicked.RemoveListener(DragStart);

            GameObject.Destroy(targetSelecter);
            GameObject.Destroy(targetTileSelecter);
        }

        public virtual void SetMode()
        {
            _inputManager.OnObjectClicked.AddListener(DragStart);

            
            targetSelecter = GameObject.Instantiate(_inputManager.entitySelecter);
            targetTileSelecter = GameObject.Instantiate(_inputManager.tileSelecter);
            targetSelecter.SetActive(false);
            targetTileSelecter.SetActive(false);

        }


        // 드래그 시작
        protected virtual void DragStart(GameObject obj)
        {
            if (!obj) return;

            if (obj.TryGetComponent<Entity>(out var entity))
            {
                // 기물이 이동 가능한지 확인
                if (!entity.team.IsAlly(_inputManager.agent.id)) return;
                _selectedEntity = entity;
                targetSelecter.SetActive(true);
                targetSelecter.transform.position = _selectedEntity.transform.position + Vector3.up * 0.1f;
                targetTileSelecter.SetActive(true);

                // 기물 이동범위 표시
                moveArea = GetMovableTiles(); // 수리 모드일때는 다른 방식으로 가져옴
                foreach (var move in moveArea)
                {
                    move.ApplyHighlight(Tile.HighLightType.Move);
                }

                _inputManager.OnMouseMove.AddListener(DragEntity);
                _inputManager.OnMouseUp.AddListener(DragEnd);
            }
        }

        // 드래그 중
        protected virtual void DragEntity(Vector2 value)
        {
            Ray ray = Camera.main.ScreenPointToRay(value);
            Plane plane = new Plane(Vector3.up, new Vector3(0, 5, 0));
            Vector3 pos = _selectedEntity.transform.position;
            if (plane.Raycast(ray, out float rayDistance))
            {
                pos = ray.GetPoint(rayDistance);
            }
            // 공격 범위 표시
            _selectedTile = _inputManager.GetClosestTile(pos, moveArea);
            targetTileSelecter.transform.position = _selectedTile.transform.position + Vector3.up * 0.1f;

            foreach (var area in attackArea)
            {
                area.RemoveHighlight(Tile.HighLightType.Attack);
            }
            attackArea = _selectedEntity.GetAttackArea(_selectedTile);
            foreach (var area in attackArea)
            {
                area.ApplyHighlight(Tile.HighLightType.Attack);
            }
        }

        // 드래그 종료
        protected virtual void DragEnd()
        {
            // 엔티티 클리어
            if (_selectedEntity != null)
            {
                // 가장 가까운 타일(선택한 타일)
                foreach (var area in attackArea)
                {
                    area.RemoveHighlight(Tile.HighLightType.Attack);
                }
                foreach (var move in moveArea)
                {
                    move.RemoveHighlight(Tile.HighLightType.Move);
                }

                DragAction();

                targetSelecter.SetActive(false);
                targetTileSelecter.SetActive(false);

                _selectedEntity = null;
            }

            _inputManager.OnMouseMove.RemoveListener(DragEntity);
            _inputManager.OnMouseUp.RemoveListener(DragEnd);
        }

        /// <summary>
        /// 이동가능한 타일 리스트 반환
        /// </summary>
        /// <returns></returns>
        protected virtual List<Tile> GetMovableTiles()
        {
            return _selectedEntity.GetMoveArea();
        }
        
        /// <summary>
        /// 입력된 데이터로 사용할 명령 입력
        /// </summary>
        protected virtual void DragAction()
        {
            // 제자리 이동 불가능
            if (!_selectedEntity.CurTile.Equals(_selectedTile) && _selectedTile)
            {
                
                var selectedEntity = GameObject.Instantiate(_inputManager.entitySelecter, _selectedEntity.transform.position + Vector3.up * 0.1f, Quaternion.identity);
                var selectedTile = GameObject.Instantiate(_inputManager.tileSelecter, _selectedTile.transform.position, Quaternion.identity);

                // 커맨드 생성
                var command = _inputManager.agent.CreateMoveCommand(_selectedEntity, _selectedTile);
                command.indicate.Add(selectedEntity);
                command.indicate.Add(selectedTile);
            }
        }
    }
}