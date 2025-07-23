using UnityEngine;

public class ShopUI : MonoBehaviour
{

    bool isOpen;
    public void ToggleUI()
    {
        isOpen = !isOpen;
        ToggleUI(isOpen);
    }
    public void ToggleUI(bool open)
    {
        if (open)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void Awake()
    {
        ToggleUI(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        //데이터 리스트를 순회해서 각각의 데이터로 프리팹 세팅 및 표시
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
