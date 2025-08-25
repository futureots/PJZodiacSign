using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

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
    public Field field;
    public intVector2 fieldPos;
    public GameObject occupiedObject;
    public Queue<GameObject> bufferedObjects;
    // 타일로 이동가능한지 
    public bool isEmpty;

    private void Awake()
    {
        originMaterials = renderer.materials.ToList();
        currentMaterials = originMaterials;
        bufferedObjects = new Queue<GameObject>();
    }
    public void InitializeTile(Field f,int x, int y)
    {
        isEmpty = true;
        field = f;
        fieldPos.x = x;
        fieldPos.y = y;
    }
    
    public void SetOccupant(GameObject e = null)
    {
        if(e != null)
        {
            if (!isEmpty)
            {
                bufferedObjects.Enqueue(occupiedObject);
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

    public void CleanupBufferedObjects()
    {
        // 밀려난 오브젝트(파괴 예정 기물, 장애물 등) 삭제
        foreach (var item in bufferedObjects)
        {
            var component = item.GetComponent<IDamageable>();
            if (component != null)
            {
                component.Dead();
            }
            else
            {
                Destroy(item);
            }
        }
        bufferedObjects.Clear();
    }
    public void ClearOccupant()
    {
        var component = occupiedObject.GetComponent<IDamageable>();
        if (component != null)
        {
            component.Dead();
        }
        else
        {
            Destroy(occupiedObject);
        }
        occupiedObject = null;
        isEmpty = true;
    }

    public void ApplyHighlight(Material material)
    {
        currentMaterials.Add(material);
        renderer.materials = currentMaterials.ToArray();
    }
    public void RemoveHighlight(Material material)
    {
        currentMaterials.Remove(material);
        renderer.materials = currentMaterials.ToArray();
    }

}
