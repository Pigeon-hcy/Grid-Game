using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public Unit selectedUnit;
    public GridManager gridManager;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(mousePosition, Vector2.zero);
            Debug.Log("Ray Sent");

            if (hit.Length > 0)
            {
                RaycastHit2D closestHit = hit.OrderBy(h => Vector2.Distance(h.point, mousePosition)).First();
                Debug.Log("Hit object: " + closestHit.collider.gameObject.name);
                Tile targetTile = closestHit.collider.GetComponent<Tile>();
                if (targetTile != null && selectedUnit != null)
                {
                    
                    selectedUnit.MoveTo(targetTile);
                    selectedUnit = null;
                    return;
                }

                Unit clickedUnit = closestHit.collider.GetComponent<Unit>();
                if (clickedUnit != null)
                {
                    selectedUnit = clickedUnit;
                    gridManager.ShowTheMovingRange(selectedUnit);
                    Debug.Log("Unit Selected: " + selectedUnit.name);
                }
            }
        }
    }


}
