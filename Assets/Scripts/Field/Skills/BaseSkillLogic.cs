using System;
using System.Collections;

[Serializable]
public abstract class BaseSkillLogic
{
    protected SkillComponent component;
    public void SetSkillComponent(SkillComponent component) => this.component = component;
    public virtual IEnumerator InputSkill(IInput input, Action<bool> callback) {
        // TODO : input을 호출해 입력을 받고 callback은 따로 구현해서 각 입력이 완료되면 true인지 false인지 확인, true면 정상 진행 false면 callback(false) 후 yield break;
        yield break; 
    }
    public virtual IEnumerator ExecuteSkill() { yield break; }

    public abstract BaseSkillLogic Clone();
}
