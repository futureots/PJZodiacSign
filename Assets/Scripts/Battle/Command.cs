using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
