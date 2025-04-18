using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Rain")]
public class rainOfSlim : ScriptableObject, unitAbility
{
    public int randomRange;
    public int damage;
    public void useEffect(Unit target)
    {
       
        int randomTime = UnityEngine.Random.Range(3,6);

        for (int i = 0; i < randomTime; i++)
        {
            randomRange = target.turnManager.PlayerList.Count + target.turnManager.EnemyList.Count;
            damage = UnityEngine.Random.Range(3, 6);
            int randomTarget = UnityEngine.Random.Range(0,randomRange);
            if (randomTarget < target.turnManager.PlayerList.Count)
            {
                target.turnManager.PlayerList[randomTarget].health -= damage;
            }
            else
            { 
                target.turnManager.EnemyList[randomTarget - target.turnManager.PlayerList.Count].health -= damage;
            }
        }
    }
}
