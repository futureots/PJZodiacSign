using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class S_Overload : BaseSkillLogic
{
    private Entity _target;
    public override async UniTask<bool> InputSkill(IInput input)
    {
        var list = StageManager.Instance.field.GetEntities();
        if (component.TryGetComponent<Entity>(out var target))
        {
            list.Remove(target);
        }
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
        var result = _target.skill.skillLogic.InputSkill(new RandomInput());
        yield return result.ToCoroutine();
        
        // 이펙트 재생
        var levelUpEffect = EffectFactory.Instance.RequestEffect("OverLoad",_target.transform.position, _target.transform.lossyScale);
        if (levelUpEffect.TryGetComponent<GlowEffect>(out var overLoad))
        {
            if (_target.TryGetComponent<MeshFilter>(out var mesh))
            {
                overLoad.Init(mesh.mesh);
                overLoad.Play();
            }
        }
        yield return new WaitForSeconds(1f);
        
        if (!result.GetAwaiter().GetResult()) yield break;
        
        yield return null;
        yield return _target.skill.skillLogic.ExecuteSkill();
        
        _target = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_Overload();
    }
}
