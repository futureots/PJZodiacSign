using Battle;

using System;
using System.Reflection;
using UnityEngine;

public class Skill : MonoBehaviour,ISkill
{
    [SkillTarget("대상 기물을 선택하세요.")]
    public Entity entity;
    [SkillTarget("대상 타일을 선택하세요.")]
    public Tile tile;

    //스킬 발동
    public virtual void Activate()
    {
        try
        {
            entity.MoveTo(tile);
            Debug.Log(tile.name + " Skill Active");
        }
        catch 
        {
            throw new Exception("Skill values arenot completed");
        }
        
    }
    
    public bool IsValidInput(FieldInfo field)
    {
        //변수 값이 할당이 안됐으면 반환
        if (field.GetValue(this) == null) return false;
        Debug.Log("Check : "+field.Name);
        //해당 변수가 타일일 때(함수를 더 구분해서 매개변수 없는 함수 여러개로 체크해도 될듯)
        if(field.Name == nameof(tile))
        {
            if (tile == null) return false;
            Debug.Log($"tile X pos = {tile.fieldPos.x}");
            if (tile.fieldPos.x == 0) return true;
            else
            {
                //해당 값이 스킬에 정해진 값을 벗어나면 null로 초기화 및 false 반환
                tile = null;
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public bool IsActable()
    {
        if(entity == null || tile == null) return false;
        return true;
    }

    public void Reinitialize()
    {
        entity = null;
        tile = null;
    }
}
