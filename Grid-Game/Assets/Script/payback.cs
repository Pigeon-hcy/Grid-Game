using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Payback")]
public class payback : ScriptableObject, unitAbility
{
    public int anger = 10;
    
    public void useEffect(Unit target)
    {
        float localDistrance = int.MaxValue;
        int index = 0;
        if (target.isEnemy == false)
        {
            for (int i = 0; i < target.turnManager.EnemyList.Count; i++)
            {
                float distance = Vector2.Distance(target.transform.position, target.turnManager.EnemyList[i].transform.position);
                if (distance < localDistrance)
                {
                    localDistrance = distance;
                    index = i;
                }
                else
                { 
                    continue;
                }
            }

            target.turnManager.EnemyList[index].health -= Mathf.Max(anger - target.health, 0);
        }


        else if (target.isEnemy == true)
        {
            for (int i = 0; i < target.turnManager.PlayerList.Count; i++)
            {
                float distance = Vector2.Distance(target.transform.position, target.turnManager.PlayerList[i].transform.position);
                if (distance < localDistrance)
                {
                    localDistrance = distance;
                    index = i;
                }
                else
                {
                    continue;
                }
            }

            target.turnManager.PlayerList[index].health -= Mathf.Max(anger - target.health, 0);
        }
    }
}
