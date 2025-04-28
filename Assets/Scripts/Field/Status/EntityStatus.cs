using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatus
{
    public Status maxHp;
    public Status power;
    public Status maxEnergy;
    
    public EntityStatus(int hp, int energy, int power)
    {
        maxHp = new Status(hp);
        this.power = new Status(power);
        maxEnergy = new Status(energy);
    }
}
