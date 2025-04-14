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
    public GameObject occupiedObject { get; private set; }
    // 타일로 이동가능한지 
    public bool isEmpty;

    private void Start()
    {
        originMaterials = renderer.materials.ToList();
        currentMaterials = originMaterials;
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
        occupiedObject = e;
        if(e != null)
        {
            e.transform.SetParent(transform);
            isEmpty = false;
        }
        else
        {
            isEmpty = true;
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("Tile Mouse DOWN");
        InputManager.Instance.OnGameObjectDown(gameObject);
    }
    private void OnMouseUp()
    {
        Debug.Log("Tile Mouse UP");
        InputManager.Instance.OnGameObjectUp();
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
