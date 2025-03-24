using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject HighLightCover;
    [SerializeField] private GameObject AttackCover;
    public bool unitOnTile;
    public bool playerOnIt;
    public int x;
    public int y;
    public int tileType = 0;
    public GridManager gridManager;
    public bool available;
    public void highLight()
    {
        HighLightCover.SetActive(true);
        available = true;
    }

    //private void OnMouseUp()
    //{
    //    if (gridManager.inMovingMode)
    //    {
    //        //Confirm movement
    //        if (HighLightCover == true)
    //        {
    //            gridManager.ConfirmMovement(this);
    //        }
    //        else
    //        {
    //            gridManager.ConfirmMovement();
    //        }

    //    }
    //}


    private void Update()
    {
        if (unitOnTile == true)
        {
            tileType = 1;
        }
        else
        {
            tileType = 0;
        }
    }

    public void ResetGrid()
    {
        AttackCover.SetActive(false);
        HighLightCover.SetActive(false);
        available = false;
    }

    public void drawAttack()
    {
        AttackCover.SetActive(true);
    }


    private void OnMouseOver()
    {
        HighLightCover.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (available == false)
        {
            HighLightCover.SetActive(false);
        }
    }
}
