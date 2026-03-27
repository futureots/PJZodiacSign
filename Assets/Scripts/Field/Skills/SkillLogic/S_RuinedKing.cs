using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class S_RuinedKing : BaseSkillLogic
{
    private Entity _owner;
    private Tile _tile;

    private int _count = 0;
    
    [SerializeField] private EntityTable _table;

    private void Init(EntityTable _table)
    {
        this._table =  _table;
    }
    public override async UniTask<bool> InputSkill(IInput input)
    {
        var list = StageManager.Instance.field.GetEntities();
        if(component.TryGetComponent<Entity>(out var owner)) { }
        else
        {
            var data = await input.InputEntity(list, 1);
            if(data != null)
            {
                owner = data[0];
            }
            else
            {
                return false;
            }
        }
        
        var tiles = owner.GetMoveArea();
        var data2 = await input.InputTile(tiles, 1);
        if (data2 == null)
        {
            return false;
        }
        
        _owner = owner;
        _tile = data2[0];
        
        return true;
    }

    public override IEnumerator ExecuteSkill()
    {
        _count++;
        var entityData = _table.table[Random.Range(0, _table.table.Count)];
        var entity = EntityFactory.Instance.Request(entityData, _owner.direction, _tile, _owner.team.teamNumber, _count / 5);
        var levelUpEffect = EffectFactory.Instance.Request("LevelUp",entity.transform.position, entity.transform.lossyScale);
        if (levelUpEffect.TryGetComponent<GlowEffect>(out var glow))
        {
            if (entity.TryGetComponent<MeshFilter>(out var mesh))
            {
                glow.Init(mesh.mesh);
                glow.Play();
            }
        }
        yield return new WaitForSeconds(0.5f);
        _tile = null;
        yield return null;
    }

    public override BaseSkillLogic Clone()
    {
        var clone = new S_RuinedKing();
        clone.Init(_table);
        return clone;
    }
}
