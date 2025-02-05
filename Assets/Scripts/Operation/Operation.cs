using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Operation : MonoBehaviour
{
    public GameObject Selecter;
    public EntityController controller;
    protected List<GameObject> markers;

    //필요한 값 받아오기
    public abstract void GetOperationValue();
    //명령 실행 default 기물 이동
    public abstract void OperatorActivate();
    protected void OnDestroy()
    {
        if (markers == null) return;
        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }
        markers.Clear();
    }
}
