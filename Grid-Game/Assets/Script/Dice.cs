using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
   

    public List<string> diceFace = new List<string>();
    public List<Sprite> diceFaceSprite = new List<Sprite>();


    public string behave;
    public int diceIndex;
    public bool isrolling;
    public bool isUsed;
    public GameObject Cover;
    [SerializeField]
    public bool isEnemy;
    [SerializeField]
    Sprite chargediceFace;
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
        diceIndex = Random.Range(0, diceFace.Count);
        GetComponent<SpriteRenderer>().sprite = diceFaceSprite[diceIndex];
        isUsed = false;
        behave = diceFace[diceIndex];

    }

    public void turnInToCharge()
    {
        behave = "Charge";
        GetComponent<SpriteRenderer>().sprite = chargediceFace;
    }

}
