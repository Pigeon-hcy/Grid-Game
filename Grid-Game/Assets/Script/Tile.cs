using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject HighLightCover;
    public bool unitOnTile;
    
    private void OnMouseOver()
    {
        HighLightCover.SetActive(true);
    }

    private void OnMouseExit()
    {
        HighLightCover.SetActive(false);
    }
}
