using System.Collections.Generic;
using UnityEngine;

public class BuffListUI : MonoBehaviour
{
    [ContextMenuItem("SetImageList", "SetImageList")]
    public List<BuffIconUI> buffIcons;


    public BuffIconUI buffIcon;
    public void SetImageList()
    {
        buffIcons = new List<BuffIconUI>();
        for(int i = 0; i < transform.childCount; i++)
        {
            var icon = transform.GetChild(i).GetComponent<BuffIconUI>();
            if (icon == null) continue;
            buffIcons.Add(icon);
        }

    }


    
    public void SetBuffUI(BuffManager buffManager)
    {
        List<BuffInstance> buffList = buffManager.BuffList;
        for (int i = 0; i < buffIcons.Count; i++)
        {
            if (buffList.Count > i)
            {
                buffIcons[i].gameObject.SetActive(true);
                buffIcons[i].SetBuffIcon(buffList[i]);
            }
            else
            {
                buffIcons[i].gameObject.SetActive(false);
            }
                
        }
    }
}
