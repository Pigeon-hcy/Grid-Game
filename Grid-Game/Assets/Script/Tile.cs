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
    public void highLight()
    {
        HighLightCover.SetActive(true);
    }
    
    private void OnMouseOver()
    {
        HighLightCover.SetActive(true);
    }

    private void OnMouseExit()
    {
        HighLightCover.SetActive(false);
    }
}
