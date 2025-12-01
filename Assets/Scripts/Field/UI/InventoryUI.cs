using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    IPhaseManageService _phaseManageService;    // TODO: Phase 의존성 주입
    InputManager _inputManager;
    /// <summary>현재 UI 표시 상태</summary>
    public bool isOpen { get; private set; } = false;
    
    #region Debugging
    public void SetClosedPosition()
    {
        closedPosition = transform.localPosition;
    }
    public void SetOpenedPosition()
    {
        openedPosition = transform.localPosition;
    }
    public void SetInventorySlot()
    {
        itemSlots = new();
        var slots = GetComponentsInChildren<ItemSlotUI>();
        itemSlots.AddRange(slots);
    }
    #endregion

    /// <summary>닫을 때 이동하는 포지션</summary>
    [ContextMenuItem("SetClosePos", "SetClosedPosition")]
    [SerializeField] Vector3 closedPosition;
    /// <summary>열 때 이동하는 포지션</summary>
    [ContextMenuItem("SetOpenPos", "SetOpenedPosition")]
    [SerializeField] Vector3 openedPosition;


    [Header("오브젝트")]
    [SerializeField] ItemInfoUI infoPanel;
    [SerializeField] ItemActionUI actPanel;
    /// <summary> 열고 닫는 버튼 컴포넌트 </summary>
    public Button popBtn;

    public Inventory inventory;     // TODO: 로컬플레이어 의존성 주입
    [ContextMenuItem("SetInvenSlot", "SetInventorySlot")]
    [SerializeField] List<ItemSlotUI> itemSlots;

    /// <summary> 아이템 데이터를 인스턴스로 전환 </summary>
    [SerializeField] ItemTable itemTable;

    public void Init(InputManager input, IPhaseManageService phaseManageService)
    {
        _inputManager = input;
        _phaseManageService = phaseManageService;
        actPanel.Init(phaseManageService);
    }


    private void Start()
    {
        ToggleInventory(false);

        // 인벤토리 데이터 불러와서 표시
        SetInventory();

        // inventory.OnItemChanged += UpdateInventory;
        _inputManager.OnObjectClicked.AddListener((x) =>
        {
            actPanel.gameObject.SetActive(false);
        });
    }
    /// <summary>
    /// 보유 아이템 데이터를 인벤토리에 세팅
    /// </summary>
    void SetInventory()
    {
        for(int i=0;i< itemSlots.Count; i++)
        {
            var slot = itemSlots[i];
            slot.SetSlotIndex(i);
            slot.OnClick += OpenItemAction;
            slot.OnMouseInOut += SetInfoUI;

        }
    }

    /// <summary>
    /// 인벤토리에 아이템 추가 시 UI에 동기화
    /// </summary>
    /// <param name="item"></param>
    public void UpdateInventory(int index ,ItemInstance item)
    {
        itemSlots[index].SetSlot(item);
    }

    /// <summary>
    /// Show/Hide InventoryUI
    /// </summary>
    public void ToggleInventory()
    {
        isOpen = !isOpen;
        ToggleInventory(isOpen);
    }

    /// <summary>
    /// Show/Hide InventoryUI
    /// </summary>
    /// <param name="isOpen">true : Open, false : Close</param>
    void ToggleInventory(bool isOpen)
    {
        var panel = GetComponent<RectTransform>();
        var pos = isOpen ? openedPosition : closedPosition;
        var scaleX = isOpen ? 1 : -1;
        popBtn.transform.localScale = new Vector3(scaleX, 1, 1);
        panel.DOLocalMove(pos, 0.5f);
        if (!isOpen)
        {
            infoPanel.gameObject.SetActive(false);
            actPanel.gameObject.SetActive(false);
        }
        this.isOpen = isOpen;
    }

    void OpenItemAction(int index)
    {
        if (inventory.items.ContainsKey(index))
        {
            actPanel.gameObject.SetActive(true);
            actPanel.transform.position = itemSlots[index].transform.position;
            actPanel.SetItemAction(inventory, index);
        }
    }
    void SetInfoUI(int index, Vector2 pos)
    {
        if (inventory.items.TryGetValue(index,out var item))
        {
            if (!infoPanel.gameObject.activeSelf)
            {
                infoPanel.gameObject.SetActive(true);
                infoPanel.SetInfo(item);
            }
            infoPanel.transform.localPosition = pos;
        }
        else
        {
            infoPanel.gameObject.SetActive(false);
        }
    }

}
