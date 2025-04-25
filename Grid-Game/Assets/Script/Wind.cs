using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/WindBlow")]
public class Wind : ScriptableObject, unitAbility
{
    public void useEffect(Unit target)
    {
        if (target.isEnemy == false)
        {
            for (int i = 0; i < target.turnManager.EnemyList.Count; i++)
            {
                if (target.turnManager.EnemyList[i].health <= 2)
                {
                    target.turnManager.EnemyList[i].health -= 2;
                }
                else
                {
                    if (target.turnManager.EnemyList[i].movement > 2)
                    {
                        target.turnManager.EnemyList[i].movement -= 1;
                        target.turnManager.EnemyList[i].health -= 2;
                    }
                    else
                    {
                        target.turnManager.EnemyList[i].health -= 2;
                    }
                    
                }
                
            }
        }
        else if (target.isEnemy == true)
        {
            for (int i = 0; i < target.turnManager.PlayerList.Count; i++)
            {
                if (target.turnManager.PlayerList[i].health <= 2)
                {
                    target.turnManager.PlayerList[i].health -= 2;
                }
                else
                {
                    if (target.turnManager.PlayerList[i].movement > 2)
                    {
                        target.turnManager.PlayerList[i].movement -= 1;
                        target.turnManager.PlayerList[i].health -= 2;
                    }
                    else
                    {
                        target.turnManager.PlayerList[i].health -= 2;
                    }

                }

            }
        }
    }
}
