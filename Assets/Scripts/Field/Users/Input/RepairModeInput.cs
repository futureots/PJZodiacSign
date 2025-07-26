using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace PlayerInput
{
    public class RepairModeInput : IModeInput
    {
        List<Tile> list;
        
        Action<InputAction.CallbackContext> bindAction;
        InputManager _inputManager;

        InputAction clickEvents;
        InputAction pointEvents;
        public RepairModeInput(InputManager input)
        {
            _inputManager = input;
            clickEvents = input.inputActions.Gameplay.Click;
            pointEvents = input.inputActions.Gameplay.Point;

            list = new List<Tile>();
            bindAction = null;
            moveArea = new();
            attackArea = new();
        }
        public void RemoveMode()
        {
            Debug.Log("RemoveRepairMode");
            clickEvents.started -= DragStart;
            clickEvents.canceled -= DragEnd;
            GameObject.Destroy(targetSelecter);
            GameObject.Destroy(targetTileSelecter);

            _inputManager.UI.shop.ToggleUI(false);
            _inputManager.UI.shop.gameObject.SetActive(false);
            
        }

        public void SetMode()
        {
            _inputManager.UI.shop.gameObject.SetActive(true);
            Debug.Log("SetRepairMode");
            clickEvents.started += DragStart;
            clickEvents.canceled += DragEnd;

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
        void DragStart(InputAction.CallbackContext context)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Debug.Log("Started");
            Ray ray = Camera.main.ScreenPointToRay(_inputManager.PointerPosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var entity = hit.collider.GetComponent<Entity>();
                if (entity != null)
                {
                    // 적인지 아닌지 구분
                    var team = _inputManager.team;
                    if (!team.isAlly(entity.team)) return;
                    _selectedEntity = entity;
                    //값이 변경될 때마다 선택한 엔티티의 위치 이동
                    bindAction = value =>
                    {
                        Ray ray2 = Camera.main.ScreenPointToRay(value.ReadValue<Vector2>());
                        DragEntity(_selectedEntity, ray2, targetTileSelecter);
                    };

                    targetSelecter.SetActive(true);
                    targetSelecter.transform.position = _selectedEntity.transform.position + Vector3.up * 0.1f;
                    targetTileSelecter.SetActive(true);
                    // 기물 이동범위 표시
                    moveArea = GameManager.Instance.field.GetHalfTiles(_inputManager.controller.isReflect);
                    moveArea.AddRange(_inputManager.controller.instantField.GetTiles());

                    _inputManager.areaVisualizer.ShowMoveArea(moveArea);
                    pointEvents.performed += bindAction;
                }
            }
        }
        // 드래그 종료
        void DragEnd(InputAction.CallbackContext context)
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

            pointEvents.performed -= bindAction;
            Debug.Log("DragEnd");
        }

        // 드래그 중
        void DragEntity(Entity entity, Ray ray, GameObject selecter)
        {
            var _areaVisualizer = _inputManager.areaVisualizer;
            Plane plane = new Plane(Vector3.up, new Vector3(0, 10, 0));
            float rayDistance;
            if (plane.Raycast(ray, out rayDistance))
            {
                Vector3 pos = ray.GetPoint(rayDistance);
                entity.transform.position = pos;
            }
            // 공격 범위 표시
            var closeTile = _inputManager.GetClosestTile(entity.transform.position, moveArea);
            selecter.transform.position = closeTile.transform.position + Vector3.up * 0.1f;
            _areaVisualizer.RemoveAttackArea(attackArea);
            attackArea = entity.GetAttackArea(closeTile);
            _areaVisualizer.ShowAttackArea(attackArea);
        }
    }
}
