using System.Collections.Generic;
using UnityEngine;

public class HpPanelManager : MonoBehaviour
{
    /**
     * 
     */
    public GameObject hpBar;

    List<GameObject> hpBarList;

    private void Awake()
    {
        hpBarList = new List<GameObject>();
    }
    /// <summary>
    ///  hp캔버스에 체력바 생성 후 오브젝트 지정
    /// </summary>
    public void CreateHpBar(Entity target)
    {

        var obj = Instantiate(hpBar);
        hpBarList.Add(obj);

        var bar = obj.GetComponent<EntityHpUI>();
        if (bar == null) return;

        bar.SetEntity(target);
        bar.hpBar.gaugeBar.color = GameManager.Instance.teamColorTable.teamColors[target.team.teamNumber];
        
    }

    public void ClearHpBar()
    {
        foreach (var item in hpBarList)
        {
            Destroy(item);
        }
        hpBarList.Clear();
    }
}
