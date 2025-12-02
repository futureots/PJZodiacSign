using TMPro;
using UnityEngine;

public class SideTile : MonoBehaviour
{
    [SerializeField] GameObject meshObj;
    [SerializeField] GameObject textObj;

    public void Initialize(string text, float rotationY = 0)
    {
        this.textObj.gameObject.SetActive(true);
        this.textObj.GetComponent<TextMeshPro>().text = text;
        transform.localEulerAngles += new Vector3(0, rotationY, 0);
        this.textObj.transform.localEulerAngles -= new Vector3(0, rotationY, 0);
    }

    public void Initialize(float rotationY = 0)
    {
        textObj.gameObject.SetActive(false);
        
        transform.localEulerAngles += new Vector3(0, rotationY, 0);
        this.textObj.transform.localEulerAngles -= new Vector3(0, rotationY, 0);
    }
}
