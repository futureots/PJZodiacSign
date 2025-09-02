using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Knight", menuName = "Scriptable Objects/EntityData/Knight")]
public class ED_Knight : EntityData
{
    public override Entity CreateEntity(int level = 0)
    {
        var entity = CreateInstance();
        entity.AddComponent<KnightArea>();
        entity.InitializeEntity(this, level);

        return entity;
    }
}
