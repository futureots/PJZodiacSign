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
            Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.z = -10;
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
}
public enum ColorType
{
    Red,
    Green,
    Blue,
    White,
    Black,
    Empty
}
