using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityGoodsUI : MonoBehaviour
{

    [SerializeField] EntityUIData data;

    [SerializeField] protected Button buyBtn;
    [SerializeField] protected TextMeshProUGUI goodsName;
    [SerializeField] protected TextMeshProUGUI price;
    [SerializeField] protected Image icon;


    public void SetGoods(EntityUIData data)
    {
        goodsName.text = data.name;
        price.text = data.normalPrice.ToString();
        icon.sprite = data.icon;
        buyBtn.onClick.AddListener(() =>
        {
            var agent = transform.root.GetComponent<Agent>();
            agent.BuyEntity(data);

            
        });
    }
}
