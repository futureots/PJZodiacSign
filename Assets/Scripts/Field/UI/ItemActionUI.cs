using PlayerInput;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionUI : MonoBehaviour
{
    private InputManager _inputManager;
    private ItemComponent _curItem;
    public Button useBtn;
    public Button discardBtn;
    public void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        var inventory = inputManager.agent.inventory;
        
        inputManager.OnModeChanged += OnModeChange;
        // 버리기 기능
        discardBtn.onClick.AddListener(() => {
            inventory.RemoveItem(_curItem);
            gameObject.SetActive(false);
            _curItem = null;
            });
        // 스킬 사용 기능
        useBtn.onClick.AddListener(() =>
        {
            _inputManager.SetInputMode(_curItem);
            gameObject.SetActive(false);
            _curItem = null;
        });

    }

    /// <summary>
    /// 각 버튼의 상호작용 설정
    /// </summary>
    public void SetItemAction(ItemComponent item)
    {
        _curItem = item;
        OnModeChange(_inputManager.curModeState);
    }
    // 스킬 사용 여부 판단
    void OnModeChange(IInputState state)
    {
        EditorLogger.Print($"{_curItem} : curMode : {_inputManager.curModeState}");
        if (_inputManager.curModeState is MoveModeInput)
        {
            if (!_curItem)
            {
                useBtn.interactable = false;
                discardBtn.interactable = false;
            }
            else if (_curItem.ItemData.useType.HasFlag(_inputManager.curTurnType) && _curItem.IsUsable())
            {
                useBtn.interactable = true;
                discardBtn.interactable = true;
            }
            else
            {
                useBtn.interactable = false;
                discardBtn.interactable = false;
            }
            
        }
        else
        {
            useBtn.interactable = false;
            discardBtn.interactable = true;
        }
    }
}
