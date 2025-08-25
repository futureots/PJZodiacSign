using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Knight", menuName = "Scriptable Objects/EntityData/Knight")]
public class ED_Knight : EntityData
{
    public override Entity CreateEntity(int level = 0)
    {
        var entity = CreateInstance();
        // 기물 공격, 이동 범위 세팅
        entity.AddComponent<KnightArea>();
        entity.InitializeEntity(this, level);

        return entity;
    }
}
