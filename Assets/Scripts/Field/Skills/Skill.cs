using Battle;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity entity;
    [SkillTarget("대상 타일을 선택하세요.")]
    public Tile tile;
    //스킬 발동
    public void Activate()
    {
        entity.MoveTo(tile);
        Debug.Log(tile.name + " Skill Active");
    }
}
