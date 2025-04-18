using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Payback")]
public class payback : ScriptableObject, unitAbility
{
    public int anger = 10;
    public void useEffect(Unit target1, Unit target2)
    {
        if (target1.isEnemy == false)
        {
            if (target1.isEnemy != target2.isEnemy)
            {
                target2.health -= Mathf.Min(0, (anger - target1.health));
            }
        }
        else if (target1.isEnemy == true)
        {
            int random;
            random = Random.Range(0, target1.turnManager.PlayerList.Count);
            target1.turnManager.PlayerList[random].health -= Mathf.Min(0, (anger - target1.health));
        }
    }
}
