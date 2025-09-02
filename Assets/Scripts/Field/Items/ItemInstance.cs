using System;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData itemData { get; private set; }
    public int count;
    public ItemInstance(ItemData itemData)
    {
        this.itemData = itemData;
    }

    public Action OnDiscard;

    /// <summary>
    /// 아이템 버리기
    /// </summary>
    public void Discard()
    {
        OnDiscard?.Invoke();
    }



}
