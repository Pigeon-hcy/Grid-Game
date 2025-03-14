using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfHealing : unitEffect
{
    public selfHealing()
        : base() 
    {
        effectName = "selfHealing";
        
    }

    public override void effect()
    {
        
    }

    public override void effect(List<Unit> targets)
    {
        
    }

    public override void effect(Tile targetTile)
    {
        
    }

    public override void effect(Unit self)
    {
        self.health += 4;
        Debug.Log("self Heal");
    }
}
