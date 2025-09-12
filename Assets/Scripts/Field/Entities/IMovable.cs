using UnityEngine;

public interface IMovable : IOccupant
{
    /// <summary>
    ///  현재 위치한 타일
    /// </summary>
    public Tile curTile { get; set; }
}

public interface IOccupant {
    
}