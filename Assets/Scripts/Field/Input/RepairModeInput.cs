using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepairModeInput : IModeInput
{
    List<Tile> list;
    GameInputActions _inputAction;
    Action<InputAction.CallbackContext> bindAction;
    InputManager _inputManager
    {
        get
        {
            return InputManager.Instance;
        }
    }
    public RepairModeInput(GameInputActions inputAction)
    {
        _inputAction = inputAction;
        list = new List<Tile>();
        bindAction = null;
    }
    public void RemoveMode()
    {
        Debug.Log("RemoveRepairMode");
        _inputAction.Gameplay.Click.started -= DragStart;
        _inputAction.Gameplay.Click.canceled -= DragEnd;
    }

    public void SetMode()
    {
        Debug.Log("SetRepairMode");
        _inputAction.Gameplay.Click.started += DragStart;
        _inputAction.Gameplay.Click.canceled += DragEnd;
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
        Debug.Log("Started");
        Ray ray = Camera.main.ScreenPointToRay(_inputManager.PointerPosition);
        if (Physics.Raycast(ray, out var hit))
        {
            var entity = hit.collider.GetComponent<Entity>();
            if (entity != null)
            {
                // 적인지 아닌지 구분
                if (!entity.CompareTag("Player")) return;
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
                moveArea = GameManager.Instance.field.GetHalfTiles(false);
                _inputManager.areaVisualizer.ShowMoveArea(moveArea);

                _inputAction.Gameplay.Point.performed += bindAction;
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
            _selectedEntity.transform.position = _selectedEntity.curTile.transform.position;
            _areaVisualizer.RemoveAttackArea(attackArea);
            _areaVisualizer.RemoveMoveArea(moveArea);

            //커맨드 생성
            _inputManager.controller.CreateCommand(_selectedEntity, tile, targetSelecter, targetTileSelecter);
            // 제자리 이동 불가능
            if (_selectedEntity.curTile.Equals(tile)) _inputManager.controller.ClearCommand();
            _selectedEntity = null;
        }
        _inputAction.Gameplay.Point.performed -= bindAction;
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
