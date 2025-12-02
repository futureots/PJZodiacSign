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
    #region Encode & Decode
    public int Encode()
    {
        return x  | (y << 10); // X를 상위 10비트, Y를 하위 10비트에 넣음
    }

    public static intVector2 Decode(int value)
    {
        int x = value & 0x3FF; // 하위 10비트 마스크
        int y = value >> 10;
        return new intVector2 (x, y);
    }

    public static intVector2 Zero => new intVector2(0, 0);
    #endregion
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

