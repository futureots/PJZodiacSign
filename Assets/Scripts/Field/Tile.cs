using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    public new Renderer renderer
    {
        get
        {
            return GetComponentInChildren<Renderer>();
        }
    }
    List<Material> originMaterials;
    List<Material> currentMaterials;
    //이 타일이 있는 필드
    public Field field { get; private set; }
    public intVector2 fieldPos;
    public GameObject occupiedObject;
    public Queue<GameObject> occupiedObjects;
    // 타일로 이동가능한지 
    public bool isEmpty;

    private void Start()
    {
        originMaterials = renderer.materials.ToList();
        currentMaterials = originMaterials;
        occupiedObjects = new Queue<GameObject>();
    }
    public void SetField(Field f,int x, int y)
    {
        isEmpty = true;
        field = f;
        fieldPos.x = x;
        fieldPos.y = y;
    }
    
    public void OccupyObject(GameObject e = null)
    {
        if(e != null)
        {
            if (!isEmpty)
            {
                occupiedObjects.Enqueue(occupiedObject);
                occupiedObject.SetActive(false);
            }
            occupiedObject = e;
            e.transform.SetParent(transform);
            isEmpty = false;
        }
        else
        {
            isEmpty = true;
            occupiedObject = null;
        }
    }

    public void AddColor(Material material)
    {
        currentMaterials.Add(material);
        renderer.materials = currentMaterials.ToArray();
    }
    public void RemoveColor(Material material)
    {
        currentMaterials.Remove(material);
        renderer.materials = currentMaterials.ToArray();
    }

}
