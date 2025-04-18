using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/FireUp")]
public class fireUp : ScriptableObject, unitAbility
{
    public int damage = 3;
    public int growAmount = 2;

    public void useEffect(Unit target)
    {
        target.health -= damage;
        target.attackDamage += growAmount;
    }
}
