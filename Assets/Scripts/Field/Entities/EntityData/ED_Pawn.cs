using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Pawn", menuName = "Scriptable Objects/EntityData/Pawn")]
public class ED_Pawn : EntityData
{
    public override Entity CreateEntity(int level = 0)
    {
        var entity = CreateInstance();
        entity.AddComponent<PawnAttackArea>();
        entity.AddComponent<PawnMoveArea>();
        entity.InitializeEntity(this, level);
        return entity;
    }
}
