using System.Collections.Generic;
using UnityEngine;

public class InputUIContainer : MonoBehaviour
{
    public List<InputManagerUI> uiList;
    public void Init(InputManager inputManager)
    {
        foreach (var ui in uiList)
        {
            ui.Init(inputManager);
        }
    }

    [ContextMenu("SetUIPanels")]
    public void SetUI()
    {
        uiList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent(out InputManagerUI ui))
            {
                uiList.Add(ui);
            }
        }
    }
}

public abstract class InputManagerUI : MonoBehaviour
{
    public abstract void Init(InputManager inputManager);
}