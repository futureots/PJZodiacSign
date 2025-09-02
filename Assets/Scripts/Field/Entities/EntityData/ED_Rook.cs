using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Rook", menuName = "Scriptable Objects/EntityData/Rook")]
public class ED_Rook : EntityData
{
    public override Entity CreateEntity(int level = 0)
    {
        var entity = CreateInstance();
        entity.AddComponent<RookArea>();
        entity.InitializeEntity(this, level);

        return entity;
    }
}
