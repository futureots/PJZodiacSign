using PlayerInput;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionUI : MonoBehaviour
{
    InputManager inputManager;
    ItemComponent curItem;
    public Button useBtn;
    public Button discardBtn;
    public void Init(InputManager _inputManager)
    {
        inputManager = _inputManager;
        var inventory = _inputManager.agent.inventory;
        
        _inputManager.onModeChanged += OnModeChange;
        // 버리기 기능
        discardBtn.onClick.AddListener(() => {
            inventory.RemoveItem(curItem);
            gameObject.SetActive(false);
            curItem = null;
            });
        // 스킬 사용 기능
        useBtn.onClick.AddListener(() =>
        {
            inputManager.SetInputMode(curItem);
            gameObject.SetActive(false);
            curItem = null;
        });

    }

    /// <summary>
    /// 각 버튼의 상호작용 설정
    /// </summary>
    public void SetItemAction(ItemComponent item)
    {
        curItem = item;
        OnModeChange(inputManager.curModeState);
    }
    // 스킬 사용 여부 판단
    void OnModeChange(IInputState state)
    {
        if ((curItem.itemData.useType & inputManager.curTurnType) != 0 && inputManager.curModeState is MoveModeInput)
        {
            useBtn.interactable = true;
        }
        else
        {
            useBtn.interactable = false;
        }
    }
}
