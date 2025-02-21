using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject HighLightCover;
    public bool unitOnTile;
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

    public void ResetGrid()
    {
        HighLightCover.SetActive(false);
        available = false;
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
