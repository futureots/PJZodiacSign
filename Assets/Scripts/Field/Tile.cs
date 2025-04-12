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
    //이 타일이 있는 필드
    public Field field { get; private set; }
    public intVector2 fieldPos;
    public GameObject occupiedObject { get; private set; }
    public bool isOccupied => occupiedObject != null;

    public void SetField(Field f,int x, int y)
    {
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
}
