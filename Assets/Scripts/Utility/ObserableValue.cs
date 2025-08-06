using System;
using UnityEngine;

[System.Serializable]
public class ObservableValue<T>
{
    [SerializeField] T value;
    public T Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            OnValueChanged?.Invoke(this.value);
        }
    }
    public Action<T> OnValueChanged;
}
