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

    /// <summary>
    /// 해당 타일의 점거 상태
    /// </summary>
    public bool isEmpty => occupiedObject == null;

    private void Awake()
    {
        originMaterials = renderer.materials.ToList();
        currentMaterials = originMaterials;
    }
    /// <summary>
    /// 필드 설정
    /// </summary>
    /// <param name="f">필드</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void InitializeTile(Field f,int x, int y)
    {
        field = f;
        fieldPos.x = x;
        fieldPos.y = y;
    }
    
    /// <summary>
    /// 타일 점거
    /// </summary>
    /// <param name="e">타일을 점거한 오브젝트</param>
    public bool SetOccupant(GameObject obj = null, bool ignoreOccupant= false)
    {
        if(isEmpty || ignoreOccupant)
        {
            Destroy(occupiedObject);

            occupiedObject = obj;
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            return true;
        }
        else
        {
            return false;
        }
    } 
    public void UnsetOccupant()
    {
        occupiedObject = null;
    }

    /// <summary>
    /// 점거한 오브젝트 제거
    /// </summary>
    public void ClearOccupant()
    {
        var component = occupiedObject.GetComponent<IDamageable>();
        UnsetOccupant();
        if (component != null)
        {
            component.Dead();
        }
        else
        {
            Destroy(occupiedObject);
        }
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
