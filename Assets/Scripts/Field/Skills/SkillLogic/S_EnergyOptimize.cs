using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_EnergyOptimize : BaseSkillLogic
{
    private Entity _target;
    public override async UniTask<bool> InputSkill(IInput input)
    {
        if (component.TryGetComponent<Entity>(out var target))
        {
            _target = target;
            return true;
        }
        var list = StageManager.Instance.field.GetEntities();
        var data = await input.InputEntity(list, 1);
        if(data != null)
        {
            _target = data[0];
            return true;
        }
        
        return false;
    }

    public override IEnumerator ExecuteSkill()
    {
        var levelUpEffect = EffectFactory.Instance.Request("OverLoad",_target.transform.position, _target.transform.lossyScale);
        if (levelUpEffect.TryGetComponent<GlowEffect>(out var overLoad))
        {
            if (_target.TryGetComponent<MeshFilter>(out var mesh))
            {
                overLoad.Init(mesh.mesh);
                overLoad.Play();
            }
        }

        _target.energy.MaxEnergy = Math.Max(_target.energy.MaxEnergy - 1, 1);

        _target = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_EnergyOptimize();
    }
}
