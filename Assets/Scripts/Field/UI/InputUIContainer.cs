using UnityEngine;

public class InputUIContainer : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] EntityInfoUI entityInfoUI;
    [SerializeField] TurnUI turnUI;
    [SerializeField] PlayerDataUI playerDataUI;
    [SerializeField] EnhanceConfirmUI enhanceConfirmUI;
    [SerializeField] SkillCancelUI skillCancelUI;
    [SerializeField] CostUI costUI;
    [SerializeField] private StageEndUI stageEndUI;
    public void Init(InputManager inputManager)
    {
        inventoryUI.Init(inputManager);
        entityInfoUI.Init(inputManager);
        turnUI.Init(inputManager);
        playerDataUI.Init(inputManager);
        enhanceConfirmUI.Init(inputManager);
        skillCancelUI.Init(inputManager);
        costUI.Init(inputManager);
        stageEndUI.Init(inputManager);
    }
}
