using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldViewer : MonoBehaviour
{
    public EntityController controller;
    public Field field;
    public Entity SelectedEntity;

    /// <summary>
    /// 예상 적 공격 범위 표시 메테리얼
    /// </summary>
    public Material dangerTileMaterial;

    /// <summary>
    /// 선택한 엔티티 예상 공격범위 표시 메테리얼
    /// </summary>
    public Material expectAttackMaterial;

    /// <summary>
    /// 선택한 엔티티 이동 가능범위 표시 메테리얼
    /// </summary>
    public Material expectMoveMaterial;

    List<Tile> enemiesAttackArea = new List<Tile>();
    List<Tile> entityMovableArea = new List<Tile>();
    List<Tile> entityAttackArea = new List<Tile>();


    void ViewArea(Material material, params Tile[] tiles)
    {

    }

    void ViewMovableArea()
    {
        if (field == null) return;

    }
}
