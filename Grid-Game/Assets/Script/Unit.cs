using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected;
    public Tile locateTile;
    public GameManager gameManager;
 
    public NewUnit newUnit;
    int movement;
    int health;

    public bool isEnemy;
    private void Awake()
    {
        
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(isEnemy == false)
            GetComponent<SpriteRenderer>().sprite = newUnit.Sprite;
        else
            GetComponent<SpriteRenderer>().sprite = newUnit.EnemySprite;
        movement = newUnit.Movement;
        health = newUnit.Health;
    }

    public void MoveTo(Tile target)
    {
        if (target.unitOnTile == false && isEnemy == false)
        {
            locateTile.unitOnTile = false;
            locateTile = null;
            target.unitOnTile = true;
            locateTile = target;
            //move the unit
            transform.position = target.transform.position;
            Debug.Log("can move");
        }
        else
        {
            Debug.Log("can't move");
            return;
        }

        
    }

    //private void OnMouseDown()
    //{
    //    gameManager.selectedUnit = this;
    //}


}
