using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shield")]
public class shield : ScriptableObject, unitAbility
{
    public int heal = 3;

    public void useEffect(Unit target)
    { 
        Vector3 Targetlocate = target.locateTile.transform.position;

        Vector3[] vectorList = new Vector3[4];
        vectorList[0] = Vector3.up;
        vectorList[1] = Vector3.down;
        vectorList[2] = Vector3.left;
        vectorList[3] = Vector3.right;

        if (target.isEnemy == true)
        {
            foreach (var dir in vectorList)
            {
                Vector3 tempLocate = Targetlocate;
                tempLocate = tempLocate + dir;

                for (int i = 0; i < target.turnManager.EnemyList.Count; i++)
                {
                    if (target.turnManager.EnemyList[i].locateTile.transform.position == tempLocate)
                    {
                        target.turnManager.EnemyList[i].health += heal;
                    }
                }


            }
        }
        else if (target.isEnemy == false) 
        {
            foreach (var dir in vectorList)
            {
                Vector3 tempLocate = Targetlocate;
                tempLocate = tempLocate + dir;

                for (int i = 0; i < target.turnManager.PlayerList.Count; i++)
                {
                    if (target.turnManager.PlayerList[i].locateTile.transform.position == tempLocate)
                    {
                        target.turnManager.PlayerList[i].health += heal;
                    }
                }


            }
        }
    }
}
