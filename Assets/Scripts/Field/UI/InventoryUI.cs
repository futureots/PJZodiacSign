using DG.Tweening;
using PlayerInput;
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
    
    [SerializeField] private List<ItemSlotUI> itemSlots;

    public void Init(InputManager input)
    {
        _inputManager = input;
        _inventory = input.agent.inventory;

        // 인벤토리 데이터 불러와서 표시
        SetInventory();

        _inventory.OnItemChanged += UpdateInventory;
    }
    
    /// <summary>
    /// 보유 아이템 데이터를 인벤토리에 세팅
    /// </summary>
    private void SetInventory()
    {
        for(int i=0;i< itemSlots.Count; i++)
        {
            var slot = itemSlots[i];
            slot.Init(i);
            slot.OnClick += UseItem;
            slot.OnMouseMove += SetInfoUI;
        }
    }

    /// <summary>
    /// 인벤토리에 아이템 추가 시 UI에 동기화
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    private void UpdateInventory(int index ,ItemComponent item)
    {
        itemSlots[index].SetSlot(item);
    }


    private void UseItem(int index)
    {
        if (_inventory.items.Count <= index)
        {
            return;
        }
        ItemComponent item = _inventory.items[index];
        if (!item)
        {
            return;
        }

        if (_inputManager.curModeState is MoveModeInput && item.ItemData.useType.HasFlag(_inputManager.curTurnType) && item.IsUsable())
        {
            _inputManager.SetInputMode(item);
        }

    }

    private void SetInfoUI(int index, Vector2 pos)
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
}
