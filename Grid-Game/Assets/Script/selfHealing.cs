using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Self Healing")]
public class selfHealing : ScriptableObject, unitAbility
{
    public int healAmount = 4;

    public void useEffect(Unit target)
    {
        target.health += healAmount;
    }

}
