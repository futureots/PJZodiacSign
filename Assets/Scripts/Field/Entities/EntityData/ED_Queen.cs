using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Queen", menuName = "Scriptable Objects/EntityData/Queen")]
public class ED_Queen : EntityData
{
    public override Entity CreateEntity(int level = 0)
    {
        var entity = CreateInstance();
        //entity.AddComponent<BishopArea>();
        //entity.AddComponent<RookArea>();
        entity.InitializeEntity(this, level);
        return entity;
    }
}
