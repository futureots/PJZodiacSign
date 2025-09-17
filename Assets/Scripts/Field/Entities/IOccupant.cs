using UnityEngine;

public interface IOccupant
{
    public Tile CurTile {  get; }
    public bool IsReflect {  get; }
}
