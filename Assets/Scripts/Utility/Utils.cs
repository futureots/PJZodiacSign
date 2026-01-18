using System;
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
[Serializable]
public struct intVector2
{
    public intVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public intVector2(intVector2 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
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

    public static intVector2 operator *(intVector2 left, intVector2 right)
    {
        return new intVector2(left.x*right.x, left.y*right.y);
    }
    public override bool Equals(object obj)
    {
        return obj is intVector2 vector &&
               x == vector.x &&
               y == vector.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public static intVector2 Zero => new intVector2(0, 0);

    public override string ToString()
    {
        return $"( x = {x}, y = {y} )";
    }
}

[Serializable]
public class Row<T>
{
    public Row()
    {
        list = new List<T>();
    }
    public List<T> list;
}

