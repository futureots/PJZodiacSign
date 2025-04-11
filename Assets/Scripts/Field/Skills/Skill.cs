using Battle;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SkillTarget("대상 기물을 선택하세요.")]
    Entity target;
    //스킬 발동
    public void Activate()
    {
        Debug.Log(target.name + " Skill Active");
    }
}
