using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Push")]
public class push : ScriptableObject, unitAbility
{

    public int distance = 2;
    public Vector3 tempLocate;
    public void useEffect(Unit target)
    {
        
        Vector3 selfLocate = target.transform.position;
        if (target.isEnemy == false)
        {
            Vector3 dir = Vector3.right;
            for (int i = 0; i < target.turnManager.EnemyList.Count; i++)
            {
                tempLocate = target.turnManager.EnemyList[i].transform.position;
                for (int j = 0; j < distance; j++)
                {
                    
                    if (target.gameManager.gridManager.vectorToTile(tempLocate).x == 9)
                    {
                        Debug.Log("break by grild Limit");
                        break;
                    }
                    Vector3 nextLocate = tempLocate + Vector3.right;
                    if (target.turnManager.EnemyList[i].gameManager.gridManager.vectorToTile(nextLocate).unitOnTile == true)
                    {
                        Debug.Log("break by enemy");
                        break;
                    }
                    tempLocate = nextLocate;
                    target.turnManager.EnemyList[i].enemyMoveToEffect(target.turnManager.EnemyList[i].gameManager.gridManager.vectorToTile(tempLocate));
                }


            }
        }
        else if (target.isEnemy == true)
        {

        }
    }
}
