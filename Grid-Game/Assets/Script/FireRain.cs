using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/RainOfFire")]
public class FireRain : ScriptableObject, unitAbility
{
    public int growAmount;

    public void useEffect(Unit target)
    {
        if (target.isEnemy == true)
        {
            for (int i = 0; i < target.turnManager.PlayerList.Count; i++)
            {
                target.turnManager.PlayerList[i].health -= target.attackDamage;

            }
        }

       if (target.isEnemy == false)
        {
            for (int i = 0; i < target.turnManager.EnemyList.Count; i++)
            {
                target.turnManager.EnemyList[i].health -= target.attackDamage;

            }
        }

        target.attackDamage += 1;
    }
}
