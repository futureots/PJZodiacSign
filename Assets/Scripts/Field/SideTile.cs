using TMPro;
using UnityEngine;

public class SideTile : MonoBehaviour
{
    [SerializeField] GameObject meshObj;
    [SerializeField] TextMeshPro text;

    public void Initialize(string text, float rotationY = 0)
    {
        this.text.gameObject.SetActive(true);
        this.text.text = text;
        meshObj.transform.localEulerAngles += new Vector3(0,rotationY,0);
    }

    public void Initialize(float rotationY = 0)
    {
        text.gameObject.SetActive(false);
        meshObj.transform.localEulerAngles += new Vector3(0, rotationY, 0);
    }
}
