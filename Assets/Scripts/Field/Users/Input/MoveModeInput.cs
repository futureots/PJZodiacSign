using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

namespace PlayerInput
{
    public class MoveModeInput : IModeInput
    {

        Action<InputAction.CallbackContext> bindAction;
        InputManager _inputManager;
        InputAction clickEvents;
        InputAction pointEvents;

        public MoveModeInput(InputManager input)
        {
            _inputManager = input;
            visualizer = input.areaVisualizer;
            clickEvents = _inputManager.inputActions.Gameplay.Click;
            pointEvents = _inputManager.inputActions.Gameplay.Point;

            attackArea = new List<Tile>();
            bindAction = null;
        }

        public void RemoveMode()
        {
            Debug.Log("RemoveMoveMode");
            clickEvents.started -= DragStart;
            clickEvents.canceled -= DragEnd;
        }

        public void SetMode()
        {
            Debug.Log("SetMoveMode");
            clickEvents.started += DragStart;
            clickEvents.canceled += DragEnd;

        }

        //선택한 엔티티 저장
        Entity _selectedEntity = null;
        GameObject targetSelecter = null;
        GameObject targetTileSelecter = null;
        List<Tile> moveArea;
        List<Tile> attackArea;
        AreaVisualizer visualizer;


        // 드래그 시작
        void DragStart(InputAction.CallbackContext context)
        {
            Debug.Log("Started");
            if (EventSystem.current.IsPointerOverGameObject()) return;
            // 마우스 위치에 ray 캐스트로 부딪힌 오브젝트 찾기
            Ray ray = Camera.main.ScreenPointToRay(_inputManager.PointerPosition);
            if (Physics.Raycast(ray, out var hit))
            {
                // 해당 오브젝트가 조작가능한 기물인지 확인
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

                    targetSelecter = UnityEngine.Object.Instantiate(_inputManager.entitySelecter, entity.transform.position + Vector3.up * 0.1f, Quaternion.identity);
                    targetTileSelecter = UnityEngine.Object.Instantiate(_inputManager.tileSelecter, entity.transform.position, Quaternion.identity);
                    // 기물 이동범위 표시
                    moveArea = entity.GetMoveArea();
                    _inputManager.areaVisualizer.ShowMoveArea(moveArea);

                    pointEvents.performed += bindAction;
                }
            }
        }
        // 드래그 종료
        void DragEnd(InputAction.CallbackContext context)
        {
            // 엔티티 클리어
            if (_selectedEntity != null)
            {
                var tile = _inputManager.GetClosestTile(_selectedEntity.transform.position, moveArea);
                _selectedEntity.transform.position = _selectedEntity.curTile.transform.position;
                visualizer.RemoveAttackArea(attackArea);
                visualizer.RemoveMoveArea(moveArea);

                // 제자리 이동 불가능
                if (!_selectedEntity.curTile.Equals(tile))
                {
                    //커맨드 생성
                    _inputManager.controller.CreateCommand(_selectedEntity, tile, targetSelecter, targetTileSelecter);
                }
                else
                {
                    GameObject.Destroy(targetSelecter);
                    GameObject.Destroy(targetTileSelecter);
                }
                _selectedEntity = null;
            }
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