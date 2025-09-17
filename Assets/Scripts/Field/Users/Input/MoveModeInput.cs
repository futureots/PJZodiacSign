using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace PlayerInput
{
    public class MoveModeInput : IModeInput
    {

        InputManager _inputManager;

        public MoveModeInput(InputManager input)
        {
            _inputManager = input;
            visualizer = input.areaVisualizer;

            attackArea = new List<Tile>();
        }

        public void RemoveMode()
        {
            _inputManager.OnObjectClicked.RemoveListener(DragStart);
            _inputManager.OnMouseUp.RemoveListener(DragEnd);
        }

        public void SetMode()
        {
            _inputManager.OnObjectClicked.AddListener(DragStart);
            _inputManager.OnMouseUp.AddListener(DragEnd);

        }

        GameObject targetSelecter = null;
        GameObject targetTileSelecter = null;

        //선택한 엔티티 저장
        Entity _selectedEntity = null;
        List<Tile> moveArea;
        List<Tile> attackArea;
        AreaVisualizer visualizer;


        // 드래그 시작
        void DragStart(GameObject obj)
        {
            if (obj == null) return;

            var entity = obj.GetComponent<Entity>();
            if (entity == null) return;

            // 적인지 아닌지 구분
            var team = _inputManager.team;
            if (!team.IsAlly(entity.team)) return;
            _selectedEntity = entity;

            targetSelecter = GameObject.Instantiate(_inputManager.entitySelecter, entity.transform.position + Vector3.up * 0.1f, Quaternion.identity);
            targetTileSelecter = GameObject.Instantiate(_inputManager.tileSelecter, entity.transform.position, Quaternion.identity);
            // 기물 이동범위 표시
            moveArea = entity.GetMoveArea();
            _inputManager.areaVisualizer.ShowMoveArea(moveArea);
            _inputManager.OnMouseMove.AddListener(DragEntity);
        }
        // 드래그 종료
        void DragEnd()
        {
            // 엔티티 클리어
            if (_selectedEntity != null)
            {
                var tile = _inputManager.GetClosestTile(_selectedEntity.transform.position, moveArea);
                _selectedEntity.transform.position = _selectedEntity.CurTile.transform.position;
                visualizer.RemoveAttackArea(attackArea);
                visualizer.RemoveMoveArea(moveArea);

                // 제자리 이동 불가능
                if (!_selectedEntity.CurTile.Equals(tile))
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