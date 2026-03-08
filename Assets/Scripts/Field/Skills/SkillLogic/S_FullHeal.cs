using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using Object = System.Object;


[Serializable]
public class S_FullHeal : BaseSkillLogic
{
    [SerializeField] private GameObject healPrefab;
    private Entity _target;
    public override async UniTask<bool> InputSkill(IInput input)
    {
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
        
        _target.CurHealth = _target.MaxHealth;
        var effect = UnityEngine.Object.Instantiate(healPrefab, _target.transform.position, Quaternion.identity);
        UnityEngine.Object.Destroy(effect,1f);
        
        yield return new WaitForSeconds(1f);
        
        _target = null;
        yield break;
    }
    public override BaseSkillLogic Clone()
    {
        return new S_FullHeal();
    }
}
