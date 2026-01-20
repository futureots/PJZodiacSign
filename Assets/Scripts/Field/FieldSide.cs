using System.Collections.Generic;
using UnityEngine;


public class FieldSide : MonoBehaviour
{
    [SerializeField] GameObject VertexOutBoard;
    [SerializeField] SideTile SideBoard;
    [SerializeField] intVector2 size;
    [SerializeField] float tileDistance;
    public bool isShowText;

    [ContextMenuItem("CreateSide", "CreateSide")]
    [ContextMenuItem("DestroySide", "DestroySide")]
    public List<GameObject> sideList;
    
    public void CreateSide()
    {
        float halfDistance = tileDistance / 2;
        int col = size.x;
        int row = size.y;
        float posX = ((col / 2) * tileDistance + halfDistance);
        float posZ = ((row / 2) * tileDistance + halfDistance);
        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                var vec = new Vector3(posX * (j == 0 ? -1 : 1), 0, (i - row / 2) * tileDistance + halfDistance);
                var rot = new Vector3(0, j * 180, 0);
                var obj = Instantiate(SideBoard, transform);
                sideList.Add(obj.gameObject);
                if (isShowText)
                {
                    obj.Initialize((i + 1).ToString());
                }
                else
                {
                    obj.Initialize();
                }

                    obj.transform.localPosition = vec;
                obj.transform.localEulerAngles = rot;
            }
        }

        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var vec = new Vector3((i - col / 2) * tileDistance + halfDistance, 0, posZ * (j == 0 ? -1 : 1));
                var rot = new Vector3(0, j * 180, 0);
                var obj = Instantiate(SideBoard, transform);
                sideList.Add(obj.gameObject);
                obj.transform.localPosition = vec;
                obj.transform.localEulerAngles = rot;
                if (isShowText)
                {
                    obj.Initialize(((char)(i + 'A')).ToString(), -90);
                }
                else
                {
                    obj.Initialize(-90);
                }
            }
        }


        for(int i = 0; i < 4; i++)
        {
            int x = 1;
            int z = 1;
            if ((i + 1) % 4 < 2) z = -1;
            if (i %4<2) x = -1;

            var vertex = Instantiate(VertexOutBoard, transform);
            sideList.Add(vertex);
            vertex.transform.localPosition = new Vector3(posX * x, 0, posZ * z);

            float angle = i * 90;
            vertex.transform.localEulerAngles = new Vector3(0, angle, 0);
        }

        

    }
    
    public void DestroySide()
    {
        foreach (var obj in sideList)
        {
            DestroyImmediate(obj);
        }
        sideList.Clear();
    }

}
