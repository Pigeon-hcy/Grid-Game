using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public string[] diceFace =
    {
        "Move",
        "Move",
        "Move",
        "Attack",
        "Attack",
        "Empty"
    };

    [SerializeField]
    Sprite[] diceFaceSprite =
    { 
        
    };
    public string behave;
    public int diceIndex;
    public bool isrolling;
    public bool isUsed;
    public GameObject Cover;
    [SerializeField]
    public bool isEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsed == true)
        {
            Cover.SetActive(true);
        }else if(isUsed == false)
        {
            Cover.SetActive(false);
        }
    }

    public void rollADice()
    {
        if (isrolling)
        {
            return;
        }
        diceIndex = Random.Range(0, diceFace.Length);
        GetComponent<SpriteRenderer>().sprite = diceFaceSprite[diceIndex];
        isUsed = false;
        behave = diceFace[diceIndex];

    }

}
