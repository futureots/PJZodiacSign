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
        
        InputManager _inputManager;

        public RepairModeInput(InputManager input)
        {
            _inputManager = input;

            moveArea = new();
            attackArea = new();
        }

        public void SetMode()
        {
            _inputManager.OnObjectClicked.AddListener(DragStart);
            _inputManager.OnMouseUp.AddListener(DragEnd);

            targetSelecter = GameObject.Instantiate(_inputManager.entitySelecter);
            targetTileSelecter = GameObject.Instantiate(_inputManager.tileSelecter);
            targetSelecter.SetActive(false);
            targetTileSelecter.SetActive(false);
        }

        public void RemoveMode()
        {
            _inputManager.OnObjectClicked.RemoveListener(DragStart);
            _inputManager.OnMouseUp.RemoveListener(DragEnd);
            GameObject.Destroy(targetSelecter);
            GameObject.Destroy(targetTileSelecter);
        }


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

            // 아군 기물인지 확인
            var team = _inputManager.team;
            if (!team.IsAlly(entity.team)) return;

            if (!_inputManager.controller.resourceEntities.Contains(entity)) return;

            _selectedEntity = entity;

            //선택된 기물 위에 선택자 표시
            targetSelecter.SetActive(true);
            targetSelecter.transform.position = _selectedEntity.transform.position + Vector3.up * 0.1f;
            
            // 기물 이동영역 표시
            moveArea = GameManager.Instance.field.GetHalfTiles(_inputManager.controller.isReflect);
            moveArea.AddRange(_inputManager.controller.resourceField.GetTiles());

            targetTileSelecter.SetActive(true);
            _inputManager.areaVisualizer.ShowMoveArea(moveArea);
            _inputManager.OnMouseMove.AddListener(DragEntity);
        }
        // 드래그 종료
        void DragEnd()
        {
            var _areaVisualizer = _inputManager.areaVisualizer;
            // 선택된 기물 클릭
            if (_selectedEntity != null)
            {
                var tile = _inputManager.GetClosestTile(_selectedEntity.transform.position, moveArea);

                _areaVisualizer.RemoveAttackArea(attackArea);
                _areaVisualizer.RemoveMoveArea(moveArea);


                // 해당 타일로 이동
                if (!_selectedEntity.Move(tile))
                {
                    var target = tile.occupiedObject.GetComponent<Entity>();
                    
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
                    _selectedEntity.transform.position = _selectedEntity.curTile.transform.position;
                }
                _selectedEntity = null;
            }
            // 선택자 제거
            targetSelecter.SetActive(false);
            targetTileSelecter.SetActive(false);

            _inputManager.OnMouseMove.RemoveListener(DragEntity);
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
            _inputManager.areaVisualizer.RemoveAttackArea(attackArea);
            _inputManager.areaVisualizer.RemoveMoveArea(moveArea);

            _inputManager.turnEndButton.interactable = false;
            _inputManager.UI.inventory.gameObject.SetActive(false);
            _inputManager.UI.confirmDialog.ShowDialog(
                "강화하시겠습니까?",
                (Action)(() => {
                    // 확인 시 강화 실행
                    Debug.Log("Enhance Confirmed");
                    target.Level += 1;
                    source.curTile.ClearOccupant();
                    _selectedEntity = null;

                    // 강화 완료 후 이벤트 구독 재설정
                    RestoreEventSubscriptions();
                    _inputManager.UI.entityInfo.HidePanel();
                }),
                (Action)(() => {
                    // 취소 시 원래 위치로 복귀
                    Debug.Log("Enhance Cancelled");
                    _selectedEntity.transform.position = _selectedEntity.curTile.transform.position;
                    _selectedEntity = null;

                    // 강화 취소 후 이벤트 구독 재설정
                    RestoreEventSubscriptions();
                })
            );
        }

        /// <summary>
        /// 이벤트 구독을 재설정합니다.
        /// </summary>
        void RestoreEventSubscriptions()
        {
            _inputManager.OnObjectClicked.AddListener(DragStart);
            _inputManager.OnMouseUp.AddListener(DragEnd);
            _inputManager.turnEndButton.interactable = true;
            _inputManager.UI.inventory.gameObject.SetActive(true);
        }

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
            var closeTile = _inputManager.GetClosestTile(_selectedEntity.transform.position, moveArea);
            targetTileSelecter.transform.position = closeTile.transform.position + Vector3.up * 0.1f;
            _areaVisualizer.RemoveAttackArea(attackArea);
            attackArea = _selectedEntity.GetAttackArea(closeTile);
            _areaVisualizer.ShowAttackArea(attackArea);
        }
    }
}
