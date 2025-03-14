using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class unitEffect
{
    public string effectName;
    public abstract void effect();

    // working on it self
    public abstract void effect(Unit target);

    public abstract void effect(Tile targetTile);

    public abstract void effect(List<Unit> targets);
}
