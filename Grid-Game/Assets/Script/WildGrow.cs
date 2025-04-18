using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Deer")]
public class WildGrow : ScriptableObject, unitAbility
{
    public int growAmount = 1;

    public void useEffect(Unit target)
    {
        target.attackDamage += growAmount;
        target.movement += growAmount;
    }
}
