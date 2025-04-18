using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Revenge")]
public class fireG : ScriptableObject, unitAbility
{

    public void useEffect(Unit target)
    {
        target.attackDamage = (10 - (target.turnManager.PlayerList.Count + target.turnManager.EnemyList.Count)) / 2 + 1; 
    }
}
