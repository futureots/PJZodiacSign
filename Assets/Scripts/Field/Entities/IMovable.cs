using UnityEngine;

public interface IMovable
{
    /// <summary>
    ///  현재 위치한 타일
    /// </summary>
    public Tile curTile { get; set; }
}
