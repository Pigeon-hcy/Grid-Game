using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Eyebeam")]
public class Eyebeam : ScriptableObject, unitAbility
{
    public int damage = 5;
    public Vector3 locate;
    public void useEffect(Unit target)
    {
        if (target.isEnemy == false)
        {
            locate = target.transform.position;
            for (int i = 0; i < target.turnManager.EnemyList.Count; i++)
            {
                if (locate.y == target.turnManager.EnemyList[i].transform.position.y && locate.x < target.turnManager.EnemyList[i].transform.position.x)
                {
                    target.turnManager.EnemyList[i].health -= damage;
                    
                }
            }

            for (int i = 0; i < target.turnManager.PlayerList.Count; i++)
            {
                if (locate.y == target.turnManager.PlayerList[i].transform.position.y && locate.x < target.turnManager.PlayerList[i].transform.position.x)
                {
                    target.turnManager.PlayerList[i].health -= damage;
                    
                }
            }

        }
        else if (target.isEnemy == true)
        {
            locate = target.transform.position;

            for (int i = 0; i < target.turnManager.EnemyList.Count; i++)
            {
                if (locate.y == target.turnManager.EnemyList[i].transform.position.y && locate.x > target.turnManager.EnemyList[i].transform.position.x)
                {
                    target.turnManager.EnemyList[i].health -= damage;
                }
            }

            for (int i = 0; i < target.turnManager.PlayerList.Count; i++)
            {
                if (locate.y == target.turnManager.PlayerList[i].transform.position.y && locate.x > target.turnManager.PlayerList[i].transform.position.x)
                {
                    target.turnManager.PlayerList[i].health -= damage;
                }
            }
        }
    }
}
