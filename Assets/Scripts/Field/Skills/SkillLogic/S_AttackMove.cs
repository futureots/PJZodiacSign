using Cysharp.Threading.Tasks;
using System;
using System.Collections;

[Serializable]
public class S_AttackMove : BaseSkillLogic
{
    private Entity _owner;
    private Tile _tile;
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
        yield return _owner.StartCoroutine(_owner.Attack());
        _owner.Move(_tile);

        _tile = null;
        _owner = null;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_AttackMove();
    }
}
