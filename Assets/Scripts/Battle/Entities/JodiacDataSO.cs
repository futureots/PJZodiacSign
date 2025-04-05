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



}

[System.Serializable]
public struct SkillData
{
    public int demandEnergy;
    public Skill skill;
}
