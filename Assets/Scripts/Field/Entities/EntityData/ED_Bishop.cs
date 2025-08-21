using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Bishop", menuName = "Scriptable Objects/EntityData/Bishop")]
public class ED_Bishop : EntityData
{
    public override Entity CreateEntity(int level = 0)
    {
        var entity = CreateInstance();
        entity.AddComponent<BishopArea>();
        entity.InitializeEntity(this, level);
        return entity;
    }
}
