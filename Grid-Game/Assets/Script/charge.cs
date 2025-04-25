using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Charge")]
public class charge : ScriptableObject, unitAbility
{
    public int damage = 5;
    public void useEffect(Unit target)
    {
        Vector3 Targetlocate = target.locateTile.transform.position;
        if (target.isEnemy == false)
        {
            Vector3 tempLocate = Targetlocate;
            Debug.Log("Enter while");
            while (true)
            {
                Debug.Log("TempLocate" + tempLocate.x);
                if (target.gameManager.gridManager.vectorToTile(tempLocate).x == 9)
                {
                    Debug.Log("break by grild Limit");
                    break;
                }

                Vector3 nextLocate = tempLocate + Vector3.right;

                if (target.gameManager.gridManager.vectorToTile(nextLocate).unitOnTile == true)
                {
                    Debug.Log("break by Hit enemy");
                    for (int i = 0; i < target.turnManager.EnemyList.Count; i++)
                    {
                        if (target.turnManager.EnemyList[i].locateTile.transform.position == nextLocate)
                        {
                            target.turnManager.EnemyList[i].health -= damage;
                        }
                    }
                    break;
                }

                tempLocate = nextLocate;
            }

            Debug.Log("moving");
            target.gameManager.gridManager.vectorToTile(tempLocate).available = true;
            target.EffectMoveTo(target.gameManager.gridManager.vectorToTile(tempLocate));
        }

        else if (target.isEnemy == true)
        {
            Vector3 tempLocate = Targetlocate;
            while (true)
            {
                if (target.gameManager.gridManager.vectorToTile(tempLocate).x == 0)
                {
                    break;
                }

                Vector3 nextLocate = tempLocate + Vector3.left;

                if (target.gameManager.gridManager.vectorToTile(nextLocate).unitOnTile == true)
                {
                    for (int i = 0; i < target.turnManager.PlayerList.Count; i++)
                    {
                        if (target.turnManager.PlayerList[i].locateTile.transform.position == nextLocate)
                        {
                            target.turnManager.PlayerList[i].health -= damage;
                        }
                    }
                    break;
                }

                tempLocate = nextLocate;
            }

            target.gameManager.gridManager.vectorToTile(tempLocate).available = true;
            target.EffectMoveTo(target.gameManager.gridManager.vectorToTile(tempLocate));

        }
    }
}
