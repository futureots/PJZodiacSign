using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "King", menuName = "Scriptable Objects/EntityData/King")]
public class ED_King : EntityData
{
    public override Entity CreateEntity(int level = 0)
    {
        var entity = CreateInstance();
        // 기물 공격, 이동 범위 세팅
        entity.AddComponent<KingArea>();
        entity.InitializeEntity(this, level);

        return entity;
    }
}
