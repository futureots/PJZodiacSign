using PlayerInput;
using System.Collections;
using UnityEngine;

public class AttackModeInput : IInputState
{
    private InputManager _input;
    public AttackModeInput(InputManager input)
    {
        _input = input;
    }
    public void SetMode()
    {
        _input.StartCoroutine(AttackInput());
    }

    public void RemoveMode()
    {
        
    }

    IEnumerator AttackInput()
    {
        yield return null;
        foreach (var entity in _input.agent.fieldEntities)
        {
            _input.agent.CreateAttackCommand(entity);
        }
        _input.agent.CreateEndCommand();
    }
}
