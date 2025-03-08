using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceLoader : MonoBehaviour
{
    [SerializeField]
    List<Dice> diceList = new List<Dice>();
    [SerializeField]
    UnitLoader unitLoader;
    public List<NewUnit> PlayerunitList;
    private void Awake()
    {
        unitLoader = GameObject.FindGameObjectWithTag("Loader").GetComponent<UnitLoader>();
        PlayerunitList = UnitLoader.Instance.Playersunits;
        for (int i = 0; i < PlayerunitList.Count; i++)
        {
            diceList[i].diceFace = PlayerunitList[i].diceStrings;
            diceList[i].diceFaceSprite = PlayerunitList[i].diceSprites;

        }

    }

}
