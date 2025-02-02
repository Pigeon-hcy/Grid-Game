using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Unit selectedUnit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            Debug.Log("Ray Sent");

            if (hit.collider != null)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
                Tile targetTile = hit.collider.GetComponent<Tile>();
                if (targetTile != null && selectedUnit != null)
                {
                    
                    selectedUnit.MoveTo(targetTile);
                    selectedUnit = null;
                    return;
                }

                Unit clickedUnit = hit.collider.GetComponent<Unit>();
                if (clickedUnit != null)
                {
                    selectedUnit = clickedUnit;
                    Debug.Log("Unit Selected: " + selectedUnit.name);
                }
            }
        }
    }


}
