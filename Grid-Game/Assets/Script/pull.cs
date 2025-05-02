using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Pull")]
public class pull : ScriptableObject, unitAbility
{
    public int distance = 2;
   
    public void useEffect(Unit target)
    {
        Vector3 tempLocate;
        Vector3 selfLocate = target.transform.position;
        if (target.isEnemy == false)
        {
            Vector3 dir = Vector3.right;
            for (int i = 0; i < target.turnManager.PlayerList.Count; i++)
            {
                if (selfLocate == target.turnManager.PlayerList[i].transform.position)
                {
                    continue;
                }
                else
                {
                    tempLocate = target.turnManager.PlayerList[i].transform.position;

                    for (int j = 0; j < distance; j++)
                    {
                        if (target.turnManager.PlayerList[i].gameManager.gridManager.vectorToTile(tempLocate).x == 9 )
                        {
                            Debug.Log("break by distance");
                            break;
                            
                        }
                        Vector3 nextLocate = tempLocate + Vector3.right;
                        if (target.turnManager.PlayerList[i].gameManager.gridManager.vectorToTile(nextLocate).unitOnTile == true)
                        {
                            Debug.Log("break by enemy");
                            break;
                        }
                        Debug.Log("no break");
                        tempLocate = nextLocate;
                        target.turnManager.PlayerList[i].gameManager.gridManager.vectorToTile(tempLocate).available = true;
                        target.turnManager.PlayerList[i].EffectMoveTo(target.turnManager.PlayerList[i].gameManager.gridManager.vectorToTile(tempLocate));
                    }

                }
            }
        }
        else if (target.isEnemy == true)
        {
            Vector3 dir = Vector3.left;
            for (int i = 0; i < target.turnManager.EnemyList.Count; i++)
            {
                if (selfLocate == target.turnManager.EnemyList[i].transform.position)
                {
                    continue;
                }
                else
                {
                    tempLocate = target.turnManager.EnemyList[i].transform.position;

                    for (int j = 0; j < distance; j++)
                    {
                        if (target.turnManager.EnemyList[i].gameManager.gridManager.vectorToTile(tempLocate).x == 0)
                        {
                            break;
                        }
                        Vector3 nextLocate = tempLocate + Vector3.left;
                        if (target.turnManager.EnemyList[i].gameManager.gridManager.vectorToTile(nextLocate).unitOnTile == true)
                        {
                            break;
                        }

                        tempLocate = nextLocate;
                        target.turnManager.EnemyList[i].turnManager.EnemyList[i].gameManager.gridManager.vectorToTile(tempLocate).available = true;
                        target.turnManager.EnemyList[i].enemyMoveToEffect(target.turnManager.EnemyList[i].gameManager.gridManager.vectorToTile(tempLocate));
                    }

                }
            }
        }
    }
}
