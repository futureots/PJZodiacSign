using System.Collections.Generic;
using UnityEngine;

public class HpPanelManager : MonoBehaviour
{
    /**
     * 
     */
    public EntityHpUI hpBar;

    List<GameObject> hpBarList;

    private void Awake()
    {
        hpBarList = new List<GameObject>();
        EntityFactory.Instance.OnEntityCreated += CreateHpBar;
    }



    /// <summary>
    ///  hp캔버스에 체력바 생성 후 오브젝트 지정
    /// </summary>
    public void CreateHpBar(Entity target)
    {

        var bar = Instantiate(hpBar,transform);
        bar.gameObject.transform.localScale = Vector3.one * 0.1f;
        hpBarList.Add(bar.gameObject);

        bar.Init(target, ()=> hpBarList.Remove(bar.gameObject));
        
    }

    public void ClearHpBar()
    {
        foreach (var item in hpBarList)
        {
            Destroy(item);
        }
        hpBarList.Clear();
    }
    private void OnDestroy()
    {
        if (EntityFactory.Instance)
        {
            EntityFactory.Instance.OnEntityCreated -= CreateHpBar;
        }
    }
}
