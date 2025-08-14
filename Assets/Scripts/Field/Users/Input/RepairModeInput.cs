using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

namespace PlayerInput
{
    public class RepairModeInput : IModeInput
    {
        
        Action<Vector2> bindAction;
        InputManager _inputManager;

        public RepairModeInput(InputManager input)
        {
            _inputManager = input;

            bindAction = null;
            moveArea = new();
            attackArea = new();
        }
        public void RemoveMode()
        {
            _inputManager.OnObjectClicked.RemoveListener(DragStart);
            _inputManager.OnMouseUp.RemoveListener(DragEnd);
            GameObject.Destroy(targetSelecter);
            GameObject.Destroy(targetTileSelecter);

            _inputManager.UI.shop.ToggleUI(false);
            _inputManager.UI.shop.gameObject.SetActive(false);
            
        }

        public void SetMode()
        {
            _inputManager.UI.shop.gameObject.SetActive(true);

            _inputManager.OnObjectClicked.AddListener(DragStart);
            _inputManager.OnMouseUp.AddListener(DragEnd);

            // 표시자 생성 삭제 => 활성화 비활성화
            targetSelecter = GameObject.Instantiate(_inputManager.entitySelecter);
            targetTileSelecter = GameObject.Instantiate(_inputManager.tileSelecter);
            targetSelecter.SetActive(false);
            targetTileSelecter.SetActive(false);

        }

        //선택한 엔티티 저장
        Entity _selectedEntity = null;
        GameObject targetSelecter = null;
        GameObject targetTileSelecter = null;
        List<Tile> moveArea;
        List<Tile> attackArea;



        // 드래그 시작
        void DragStart(GameObject obj)
        {
            if (obj == null) return;
            
            var entity = obj.GetComponent<Entity>();
            if (entity == null) return;
            // 적인지 아닌지 구분
            var team = _inputManager.team;
            if (!team.isAlly(entity.team)) return;
            _selectedEntity = entity;
            //값이 변경될 때마다 선택한 엔티티의 위치 이동

            targetSelecter.SetActive(true);
            targetSelecter.transform.position = _selectedEntity.transform.position + Vector3.up * 0.1f;
            
            // 기물 이동범위 표시
            moveArea = GameManager.Instance.field.GetHalfTiles(_inputManager.controller.isReflect);
            moveArea.AddRange(_inputManager.controller.instantField.GetTiles());

            targetTileSelecter.SetActive(true);
            _inputManager.areaVisualizer.ShowMoveArea(moveArea);
            _inputManager.OnMouseMove.AddListener(DragEntity);
        }
        // 드래그 종료
        void DragEnd()
        {
            var _areaVisualizer = _inputManager.areaVisualizer;
            // 엔티티 클리어
            if (_selectedEntity != null)
            {
                var tile = _inputManager.GetClosestTile(_selectedEntity.transform.position, moveArea);

                _areaVisualizer.RemoveAttackArea(attackArea);
                _areaVisualizer.RemoveMoveArea(moveArea);


                // 해당 타일로 이동
                if (!_selectedEntity.MoveSequence(tile, true))
                {
                    // 실패하면 전 타일로 이동
                    _selectedEntity.transform.position = _selectedEntity.curTile.transform.position;
                }
                    _selectedEntity = null;
            }
            // 표시자 제거
            targetSelecter.SetActive(false);
            targetTileSelecter.SetActive(false);

            _inputManager.OnMouseMove.RemoveListener(DragEntity);
        }

        // 드래그 중
        void DragEntity(Vector2 value)
        {
            Ray ray = Camera.main.ScreenPointToRay(value);
            var _areaVisualizer = _inputManager.areaVisualizer;
            Plane plane = new Plane(Vector3.up, new Vector3(0, 10, 0));
            float rayDistance;
            if (plane.Raycast(ray, out rayDistance))
            {
                Vector3 pos = ray.GetPoint(rayDistance);
                _selectedEntity.transform.position = pos;
            }
            // 공격 범위 표시
            var closeTile = _inputManager.GetClosestTile(_selectedEntity.transform.position, moveArea);
            targetTileSelecter.transform.position = closeTile.transform.position + Vector3.up * 0.1f;
            _areaVisualizer.RemoveAttackArea(attackArea);
            attackArea = _selectedEntity.GetAttackArea(closeTile);
            _areaVisualizer.ShowAttackArea(attackArea);
        }
    }
}
