using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ChangeToCharge")]
public class changeToCharge : ScriptableObject, unitAbility
{
    public int changenumber = 2;
    public void useEffect(Unit target)
    {
        
        if (target.isEnemy == false)
        {
            for (int i = 0; i < target.turnManager.Playerdices.Length; i++)
            {
                if (changenumber != 0)
                {
                    if (target.turnManager.Playerdices[i].isUsed == false && target.turnManager.Playerdices[i].behave == "Move")
                    {
                        target.turnManager.Playerdices[i].turnInToCharge();
                    }
                }
                else if (changenumber == 0)
                { 
                    break;
                }
            }
        }
        else if (target.isEnemy == true)
        {
            for (int i = 0; i < target.turnManager.EnemyDice.Length; i++)
            {
                if (changenumber != 0)
                {
                    if (target.turnManager.EnemyDice[i].isUsed == false && target.turnManager.EnemyDice[i].behave == "Move")
                    {
                        target.turnManager.EnemyDice[i].turnInToCharge();
                    }
                }
                else if (changenumber == 0)
                {
                    break;
                }
            }
        }
    }
}
