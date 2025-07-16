using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


namespace PlayerInput
{
    public class InputManager : Agent
    {
        //false일때 입력을 받고 true이면 입력을 받지 않음
        
        public Vector2 PointerPosition { get; private set; }

        public GameInputActions inputActions { get; private set; }


        public Mode currentMode;
        IModeInput curModeState;

        // 입력 표시자
        public AreaVisualizer areaVisualizer;

        // 플레이어 컨트롤러
        public EntityController controller;
        public override Command GetCommand()
        {
            var cmd = controller.curCmd;
            controller.curCmd = null;
            return cmd;
        }
        // 표시 이펙트
        public GameObject entitySelecter;
        public GameObject tileSelecter;
        public GameObject skillSelecter;

        // UI 패널
        [Header("UI Element")]
        public EntityInfoPanel entityInfoPanel;
        public Button turnEndButton;
        public UIContainer UI;
        //public GameObject cam;
        #region InputMode

        /// <summary>
        /// 모드 변경 및 입력 세팅(스킬 입력은 제외)
        /// </summary>
        public void SetInputMode(Mode mode)
        {

            curModeState?.RemoveMode();
            currentMode = mode;
            switch (mode)
            {
                case Mode.Move:
                    curModeState = new MoveModeInput(this);
                    break;
                case Mode.Repair:
                    curModeState = new RepairModeInput(this);
                    break;
                case Mode.None:
                    curModeState = new EmptyModeInput();
                    break;
            }
            curModeState.SetMode();
        }
        public void SetInputMode(IActive active)
        {
            curModeState?.RemoveMode();
            currentMode = Mode.Active;
            curModeState = new SkillModeInput(this, active);
            curModeState.SetMode();
        }
        public override void SetMode(Mode mode, Action call)
        {
            SetInputMode(mode);
            turnEndButton.onClick.AddListener(() =>
            {
                turnEndButton.onClick.RemoveAllListeners();
                call?.Invoke();
            });
        }
        #endregion

        #region ClickInfo
        /// <summary>
        /// 클릭 시 Ray로 부딪힌 기물의 정보 UI 표시하기
        /// </summary>
        private void HandleClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(PointerPosition);
            // 부딪힌 기물, (타일) UI 표시 
            if (Physics.Raycast(ray, out var hit))
            {
                var entity = hit.collider.GetComponent<Entity>();
                if (entity != null)
                {
                    Debug.Log($"Show {entity.name}'s Info");
                    // UI 표시
                    entityInfoPanel.ShowPanel(entity);
                }
                // 다른 클릭 가능한 오브젝트 확인
            }
            else
            {
                Debug.Log("Hide");
                entityInfoPanel.HidePanel();
            }

        }



        #endregion


        private void Awake()
        {
            inputActions = new GameInputActions();
        }
        private void OnEnable() => inputActions.Enable();
        private void OnDisable() => inputActions.Disable();

        private void Start()
        {
            inputActions.Gameplay.Point.performed += value => PointerPosition = value.ReadValue<Vector2>();
            inputActions.Gameplay.Click.started += _ => HandleClick();
        }

        List<Tile> list = new List<Tile>();

        public Tile GetClosestTile(Vector3 pos, List<Tile> tiles)
        {
            float minDistance = 0;
            Tile closestTile = null;
            foreach (Tile tile in tiles)
            {
                var distance = (tile.transform.position - pos).magnitude;
                if (closestTile == null || minDistance > distance)
                {
                    minDistance = distance;
                    closestTile = tile;
                }
            }
            return closestTile;
        }
    }
}