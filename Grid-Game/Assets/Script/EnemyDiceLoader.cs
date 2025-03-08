using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiceLoader : MonoBehaviour
{
    [SerializeField]
    List<Dice> diceList = new List<Dice>();
    [SerializeField]
    UnitLoader unitLoader;
    public List<NewUnit> EnemyUnitList;
    private void Awake()
    {
        unitLoader = GameObject.FindGameObjectWithTag("Loader").GetComponent<UnitLoader>();
        EnemyUnitList = UnitLoader.Instance.Enemyunits;
        for (int i = 0; i < EnemyUnitList.Count; i++)
        {
            diceList[i].diceFace = EnemyUnitList[i].diceStrings;
            diceList[i].diceFaceSprite = EnemyUnitList[i].diceSprites;

        }

    }

}
