using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPanelManager : Singleton<HpPanelManager>
{
    public GameObject hpBar;
    /// <summary>
    ///  hp캔버스에 체력바 생성 후 오브젝트 지정
    /// </summary>
    public void CreateHpBar(GameObject target)
    {
        var obj = Instantiate(hpBar,transform);
        var bar = obj.GetComponent<HpBar>();
        if (bar == null) return;
        bar.SetHpBar(target);

    }
}
