using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected;
    public Tile locateTile;
    public GameManager gameManager;
    public enum UnitType { 
        warrior,
        archer,
        wizard,
        rider,
        gunner
    }

    public UnitType type = UnitType.warrior;
    public int movement;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        switch (type)
        {
            case UnitType.warrior:
                movement = 4; 
                break;
            case UnitType.archer:
                movement = 3;
                break;
            case UnitType.wizard:
                movement = 2;
                break;
            case UnitType.rider:
                movement = 5;
                break;
            case UnitType.gunner:
                movement = 2;
                break;
        }
    }

    public void MoveTo(Tile target)
    {
        if (target.unitOnTile == false)
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
