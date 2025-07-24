using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Rook", menuName = "Scriptable Objects/EntityData/Rook")]
public class ED_Rook : EntityData
{
    public override Entity CreateEntity(int level = 0)
    {
        var entity = CreateInstance();
        // 기물 공격, 이동 범위 세팅
        entity.AddComponent<RookArea>();
        entity.SetEntity(this, level);

        return entity;
    }
}
