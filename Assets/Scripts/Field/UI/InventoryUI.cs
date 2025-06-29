using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
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
    public Vector3 closedPosition;
    /// <summary>열 때 이동하는 포지션</summary>
    [ContextMenuItem("SetOpenPos", "SetOpenedPosition")]
    public Vector3 openedPosition;


    [Header("오브젝트")]
    public ItemInfoUI infoPanel;
    public ActionMenu actPanel;

    /// <summary> 열고 닫는 버튼 컴포넌트 </summary>
    public Button popBtn;
    
    /// <summary> 아이템 데이터를 인스턴스로 전환 </summary>
    [SerializeField] ItemTable itemTable;
    [ContextMenuItem("SetInvenSlot", "SetInventorySlot")]
    [SerializeField] List<ItemSlotUI> itemSlots;
    
    private void Start()
    {
        PopInventory(false);

        //인벤토리 데이터 불러와서 표시
        SetInventory();

    }
    /// <summary>
    /// 보유 아이템 데이터를 인벤토리에 세팅
    /// </summary>
    void SetInventory()
    {
        // 아이템 데이터 가져오기
        var list = DataManager.Instance.playerData.items;
        Debug.Log($"{list.Count} + {itemSlots.Count}");
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (list.Count <= i)
            {
                itemSlots[i].ClearSlot();
                continue;
            }
            // 테이블에서 아이템 서치(없으면 다음)
            var data = itemTable.SearchItem(list[i]);
            if (data == null)
            {
                itemSlots[i].ClearSlot();
                continue;
            }

            ItemInstance instance = data.CreateInstance();
            // 인벤토리 한 칸에 세팅
            itemSlots[i].SetSlot(instance);
            itemSlots[i].OnClick += OpenItemAction;
            itemSlots[i].OnMouseInOut += SetInfoUI;
        }

    }

    /// <summary>
    /// 인벤토리 빈 자리에 아이템 추가(없으면 false 반환)
    /// </summary>
    /// <param name="item"></param>
    public bool AddItem(ItemInstance item)
    {
        var slot = GetEmptySlot();
        if (slot == null) return false;
        slot.SetSlot(item);
        return true;
    }

    ItemSlotUI GetEmptySlot()
    {
        foreach(var slot in itemSlots)
        {
            if (slot.item == null) return slot;
        }
        return null;
    }

    /// <summary>
    /// Show/Hide InventoryUI
    /// </summary>
    public void PopInventory()
    {
        isOpen = !isOpen;
        PopInventory(isOpen);
    }

    /// <summary>
    /// Show/Hide InventoryUI
    /// </summary>
    /// <param name="isOpen">true : Open, false : Close</param>
    void PopInventory(bool isOpen)
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

    }

    void OpenItemAction(ItemSlotUI slot)
    {
        actPanel.gameObject.SetActive(true);
        actPanel.transform.position = slot.transform.position;
        actPanel.SetItemAction(slot);

        //Debug.Log("OpenItemUI");
    }
    void SetInfoUI(ItemInstance item, Vector2 pos)
    {
        if (item == null)
        {
            infoPanel.gameObject.SetActive(false);
        }
        else
        {
            if (!infoPanel.gameObject.activeSelf)
            {
                infoPanel.gameObject.SetActive(true);
                infoPanel.SetInfo(item);
            }
            infoPanel.transform.position = pos;
        }
    }
}
