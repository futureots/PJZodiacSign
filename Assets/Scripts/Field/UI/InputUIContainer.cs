using UnityEngine;

public class InputUIContainer : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] EntityInfoUI entityInfoUI;
    [SerializeField] TurnUI turnUI;
    [SerializeField] PlayerDataUI playerDataUI;

    public void Init(InputManager inputManager)
    {
        inventoryUI.Init(inputManager);
        entityInfoUI.Init(inputManager);
        turnUI.Init(inputManager);
        playerDataUI.Init(inputManager.agent);
    }
}
