using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Renderer tileRenderer;
    Renderer planeRenderer;

    public enum HighLightType
    {
        Move,
        Attack,
    }
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
        tileRenderer = transform.GetChild(0).GetComponent<Renderer>();
        planeRenderer = transform.GetChild(1).GetComponent<Renderer>();
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
    
    /// <summary>
    /// 점거한 오브젝트 해제(제거가 필요할 경우 ClearOccupant 사용)
    /// </summary>
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

    #region Indicator

    public Material moveHighLightMat;
    public Material atkHighLightMat;
    public void ApplyHighlight(HighLightType type)
    {
        List<Material> mats = null;
        switch (type)
        {
            case HighLightType.Move:
                mats = tileRenderer.sharedMaterials.ToList();
                mats.Add(moveHighLightMat);
                tileRenderer.materials = mats.ToArray();
                break;
            case HighLightType.Attack:
                mats = planeRenderer.sharedMaterials.ToList();
                mats.Add(atkHighLightMat);
                planeRenderer.materials = mats.ToArray();
                break;
        }
    }
    public void RemoveHighlight(HighLightType type)
    {
        List<Material> mats = null;
        switch (type)
        {
            case HighLightType.Move:
                mats = tileRenderer.sharedMaterials.ToList();
                mats.Remove(moveHighLightMat);
                tileRenderer.materials = mats.ToArray();
                break;
            case HighLightType.Attack:
                mats = planeRenderer.sharedMaterials.ToList();
                mats.Remove(atkHighLightMat);
                planeRenderer.materials = mats.ToArray();
                break;
        }
    }

    #endregion
}
