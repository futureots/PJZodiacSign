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

    private void Start()
    {
        SetGoods(data);
    }

    public void SetGoods(EntityUIData data)
    {
        goodsName.text = data.name;
        price.text = data.normalPrice.ToString();
        icon.sprite = data.icon;
        buyBtn.onClick.AddListener(() =>
        {
            if(DataManager.Instance.playerData.credit >= data.normalPrice)
            {
                DataManager.Instance.playerData.credit -= data.normalPrice;
                var entity = ResourceManager.CreateEntity(data.id);
                var input = transform.root.GetComponent<InputManager>();
                input.controller.PlaceEntity(entity, input.controller.instantField);
            }
            
        });
    }
}
