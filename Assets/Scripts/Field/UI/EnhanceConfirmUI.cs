using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using PlayerInput;
using UnityEngine.Rendering.Universal;

public class EnhanceConfirmUI : InputManagerUI
{
    [Header("UI Elements")]
    [SerializeField] GameObject enhancePanel;
    public Button confirmButton;
    public Button cancelButton;

    InputManager inputManager;
    public override  void Init(InputManager inputManager)
    {
        this.inputManager = inputManager;
        // 강화 Input 세팅
        inputManager.OnModeChanged += OnModeChange;
    }

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
    }
    void OnModeChange(IInputState state)
    {
        if(state is RepairModeInput repair)
        {
            repair.onEnhanceRequested += OnEnhanceRequest;
        }
    }

    Entity target, source;

    void OnEnhanceRequest(Entity target, Entity source)
    {
        this.target = target;
        this.source = source;
        enhancePanel.SetActive(true);
        inputManager.ClearInputMode();
        
    }
    
    private void OnConfirm()
    {
        inputManager.agent.CreateEnhanceCommand(target, source);
        enhancePanel.SetActive(false);
        inputManager.SetInputMode();

    }
    
    private void OnCancel()
    {
        enhancePanel.SetActive(false);
        inputManager.SetInputMode();
    }
}
