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

    /// <summary>
    /// 해당 타일이 존재하는 필드
    /// </summary>
    public Field field;
    public intVector2 fieldPos;
    public GameObject occupiedObject;
    public Queue<GameObject> bufferedObjects;

    /// <summary>
    /// 해당 타일의 점거 상태
    /// </summary>
    public bool isEmpty;

    private void Awake()
    {
        originMaterials = renderer.materials.ToList();
        currentMaterials = originMaterials;
        bufferedObjects = new Queue<GameObject>();
    }
    /// <summary>
    /// 필드 설정
    /// </summary>
    /// <param name="f">필드</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void InitializeTile(Field f,int x, int y)
    {
        isEmpty = true;
        field = f;
        fieldPos.x = x;
        fieldPos.y = y;
    }
    
    /// <summary>
    /// 타일 점거
    /// </summary>
    /// <param name="e">타일을 점거한 오브젝트</param>
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

    /// <summary>
    /// 밀려난 오브젝트(파괴 예정 기물, 장애물 등) 삭제
    /// </summary>
    public void CleanupBufferedObjects()
    {
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

    /// <summary>
    /// 점거한 오브젝트 제거
    /// </summary>
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
