using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Quaternion QI => Quaternion.identity;

    public static Vector3 MousePos
    {
        get
        {
            var originPos = Input.mousePosition;
            originPos.z = Mathf.Abs(Camera.main.transform.position.y-0);
            Vector3 result = Camera.main.ScreenToWorldPoint(originPos);
            result.y = 0;
            return result;
        }
    }

}
[System.Serializable]
public struct intVector2
{
    public intVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public int x;
    public int y;
    public static intVector2 operator +(intVector2 left, intVector2 right)
    {
        return new intVector2(left.x + right.x, left.y + right.y);
    }
    public static intVector2 operator -(intVector2 left, intVector2 right)
    {
        return new intVector2(left.x - right.x, left.y - right.y);
    }
    public static bool operator ==(intVector2 left, intVector2 right)
    {
        return left.x == right.x && left.y == right.y;
    }
    public static bool operator !=(intVector2 left, intVector2 right)
    {
        return left.x != right.x || left.y != right.y;
    }
    public static intVector2 operator *(intVector2 left, int right)
    {
        return new intVector2(left.x*right, left.y*right);
    }
}
public enum Element
{
    //상성 없음(항상 최상위? 최하위?)
    Empty = 0,
    Tree=1,
    Fire = 2,
    Dirt = 3,
    Metal = 4,
    Water =5,
}
public enum Jodiac
{
    None =0,
    Mouse =1,
    Cow=2,
    Tiger=3,
    Rabbit=4,
    Dragon=5,
    Snake=6,
    Horse=7,
    Sheep=8,
    Monkey=9,
    Chicken=10,
    Dog=11,
    Pig=12
}
