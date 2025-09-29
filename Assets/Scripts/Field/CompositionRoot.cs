using System.Collections.Generic;
using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] PhaseManager phaseManager;

    [Header("Field")]
    [SerializeField] Field mainField;
    [SerializeField] List<Field> resourcefields;

    [SerializeField] InputManager inputManager;
    [SerializeField] EnemyAI enemyAI;

    [Header("UI")]
    [SerializeField] EntityInfoUI entityInfoUI;
    [SerializeField] ShopUI shopUI;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] TurnLogUI turnLogUI;

    private void Awake()
    {
        phaseManager.Init(mainField);
        inputManager.Init(phaseManager);

        entityInfoUI.Init(inputManager, phaseManager);
        shopUI.Init(inputManager,phaseManager);
        inventoryUI.Init(inputManager, phaseManager);
        turnLogUI.Init(phaseManager);
    }

}
