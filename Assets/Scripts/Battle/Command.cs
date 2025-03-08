using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class Command
{
    public Entity entity;
    public Tile tile;
    public Skill skill;
    public enum Mode
    {
        Move,
        Skill,
    }
    public Mode mode;
    public Command(Entity entity, Tile tile)
    {
        mode = Mode.Move;
        this.entity = entity;
        this.tile = tile;
    }
    public Command(Skill skill, Entity entity)
    {
        mode = Mode.Skill;
        this.skill = skill;
        this.entity = entity;
    }
    public Command(Skill skill, Tile tile)
    {
        mode = Mode.Skill;
        this.skill = skill;
        this.tile = tile;
    }

    /// <summary>
    /// 실행 시 Tile위치로 Entity를 이동가능한지 확인 후 이동
    /// </summary>
    public void Execute()
    {
        if (entity == null) return;
        if(tile == null) return;

    }


}
