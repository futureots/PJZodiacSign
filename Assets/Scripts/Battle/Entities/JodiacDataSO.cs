using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JodiacData",menuName = "JodiacData")]
public class JodiacDataSO : ScriptableObject
{
    public GameObject entityPrefab;
    // ¸ðµ¨ ¸Å½Ã
    public Mesh mesh;

    public Material[] elementMaterial;
    public int hp;
    public int hpIncrease;
    public int power;
    public int powerIncrease;
    public List<SkillData> skills;

    public Entity newEntity(int level, Element element)
    {
        var entityObj = Instantiate(entityPrefab);
        var entity = entityObj.GetComponent<Entity>();
        entity.level = level;
        //entity.elementType = element;
        return entity;
        
    }

    public List<Tile> GetAttackArea(Field field, intVector2 pos) 
    {
        var curTile = field.GetTile(pos);
        var list = new List<Tile>();
        if (curTile == null) return null;
        for(int i = 1; i >= -1; i--)
        {
            for(int j = 1; j >= -1; j--)
            {
                if (i == 0 && j == 0) continue;
                var tile = field.GetTile(pos + new intVector2(i, j));
                if(tile !=null) list.Add(tile);
            }
        }
        return null; 
    }
}

[System.Serializable]
public struct SkillData
{
    public int demandEnergy;
    public Skill skill;
}
