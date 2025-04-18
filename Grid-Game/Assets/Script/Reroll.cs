using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Reroll")]
public class Reroll : ScriptableObject, unitAbility
{

    public void useEffect(Unit target)
    {
        if (target.isEnemy == false)
        {
            for (int i = 0; i < target.turnManager.Playerdices.Length; i++)
            {
                if (target.turnManager.Playerdices[i].isUsed == false)
                {
                    target.turnManager.Playerdices[i].rollADice();
                }
            }
        }
        else if (target.isEnemy == true)
        {
            for (int i = 0; i < target.turnManager.EnemyDice.Length; i++)
            {
                if (target.turnManager.EnemyDice[i].isUsed == false)
                {
                    target.turnManager.EnemyDice[i].rollADice();
                }
            }
        }
    }
}
