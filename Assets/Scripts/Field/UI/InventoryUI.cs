using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private InputManager _inputManager;
    private Inventory _inventory;
    /// <summary>현재 UI 표시 상태</summary>
    private bool IsOpen { get; set; } = false;

    [Header("오브젝트")]
    [SerializeField] private ItemInfoUI infoPanel;
    [SerializeField] private ItemActionUI actPanel;

    /// <summary> 열고 닫는 버튼 컴포넌트 </summary>
    public Button popBtn;

    [ContextMenuItem("SetInvenSlot", "SetInventorySlot")]
    [SerializeField] private List<ItemSlotUI> itemSlots;

    public void Init(InputManager input)
    {
        _inventory = input.agent.inventory;
        actPanel.Init(input);

        // 인벤토리 데이터 불러와서 표시
        SetInventory();

        _inventory.OnItemChanged += UpdateInventory;
    }


    private void Start()
    {
        ToggleInventory(false);
    }
    /// <summary>
    /// 보유 아이템 데이터를 인벤토리에 세팅
    /// </summary>
    void SetInventory()
    {
        for(int i=0;i< itemSlots.Count; i++)
        {
            var slot = itemSlots[i];
            slot.Init(i);
            slot.onClick += OpenItemAction;
            slot.onMouseMove += SetInfoUI;
        }
    }

    /// <summary>
    /// 인벤토리에 아이템 추가 시 UI에 동기화
    /// </summary>
    /// <param name="item"></param>
    public void UpdateInventory(int index ,ItemComponent item)
    {
        itemSlots[index].SetSlot(item);
    }

    /// <summary>
    /// Show/Hide InventoryUI
    /// </summary>
    public void ToggleInventory()
    {
        IsOpen = !IsOpen;
        ToggleInventory(IsOpen);
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
        this.IsOpen = isOpen;
    }

    void OpenItemAction(int index)
    {
        if (_inventory.items.Count > index)
        {
            var item = _inventory.items[index];
            if (item != null)
            {
                actPanel.gameObject.SetActive(true);
                actPanel.transform.position = itemSlots[index].transform.position;
                actPanel.SetItemAction(item);
            }
        }

    }
    void SetInfoUI(int index, Vector2 pos)
    {
        if(index >= _inventory.items.Count || index < 0)
        {
            infoPanel.gameObject.SetActive(false);
        }
        else if (_inventory.items[index] == null)
        {
            infoPanel.gameObject.SetActive(false);
        }
        else
        {
            var item = _inventory.items[index];
            if(!infoPanel.gameObject.activeSelf) infoPanel.gameObject.SetActive(true);
            infoPanel.SetInfo(item);
            infoPanel.SetPosition(pos);
            
        }
    }


    #region Debugging

    /// <summary>닫을 때 이동하는 포지션</summary>
    [ContextMenuItem("SetClosePos", "SetClosedPosition")]
    [SerializeField] Vector3 closedPosition;
    /// <summary>열 때 이동하는 포지션</summary>
    [ContextMenuItem("SetOpenPos", "SetOpenedPosition")]
    [SerializeField] Vector3 openedPosition;


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
}
