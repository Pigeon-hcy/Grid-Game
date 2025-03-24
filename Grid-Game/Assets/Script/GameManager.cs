using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using static TurnManager;
public class GameManager : MonoBehaviour
{
    public Unit selectedUnit;
    public Unit secondUnit;
    public Dice selectedDice;
    public GridManager gridManager;
    public TurnManager turnManager;
    public GameManager gameManager;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(mousePosition, Vector2.zero);

            if (hit.Length > 0)
            {
                RaycastHit2D closestHit = hit.OrderBy(h => Vector2.Distance(h.point, mousePosition)).First();
                Tile targetTile = closestHit.collider.GetComponent<Tile>();
                Unit clickedUnit = closestHit.collider.GetComponent<Unit>();

                ///////////////////////////Move////////////////////////////////
                if (targetTile != null && selectedUnit != null)
                {
                    //excute the current behave
                    if (turnManager.currentBehave == "Move")
                    {
                        selectedUnit.MoveTo(targetTile);
                        selectedUnit = null;
                        //turnManager.excuteTheBehave();
                        return;
                    }

                    if (turnManager.currentBehave == "Charge")
                    {


                        selectedUnit.ChargeTo(targetTile);
                        turnManager.currentBehave = null;
                        selectedDice.isUsed = true;
                        if (selectedUnit.CheckForEnemies(selectedUnit.attackRange) == true)
                        {
                            turnManager.currentBehave = "Attack";
                        }
                        else
                        {
                            turnManager.excuteTheBehave();
                        }

                    }

                    


                }
                ///////////////////////Unit//////////////////////
                if (clickedUnit != null)
                {

                    //preview and select a unit
                    if (turnManager.currentBehave == "Move")
                    {
                        selectedUnit = clickedUnit;
                        gridManager.ShowTheMovingRange(selectedUnit);
                        Debug.Log("Unit Selected: " + selectedUnit.name);
                    }

                    if (turnManager.currentBehave == "Charge")
                    {
                        selectedUnit = clickedUnit;
                        gridManager.ShowTheMovingRange(selectedUnit);
                        Debug.Log(clickedUnit);
                    }


                    ////////////////////////Attack/////////////////////////////
                    if (turnManager.currentBehave == "Attack" && !clickedUnit.isEnemy)
                    {
                        if (selectedUnit != clickedUnit)
                        {
                            selectedUnit = clickedUnit;
                            selectedUnit.drawAttackRange();
                        }
                    }

                    if (clickedUnit.isEnemy && turnManager.currentBehave == "Attack" && selectedUnit != null)
                    {

                        if (selectedUnit != null && selectedUnit.CheckForEnemies(selectedUnit.attackRange))
                        {
                           secondUnit = clickedUnit;
                        }

                    }

                    if (secondUnit != null && selectedUnit != null)
                    {
                        selectedUnit.Attack(secondUnit);
                        selectedUnit = null;
                        secondUnit = null;
                        gridManager.ResetTile();
                        return;
                    }



                    //////////////////////Effect//////////////////////////
                    if (turnManager.currentBehave == "Effect")
                    {

                        selectedUnit = clickedUnit;
                        clickedUnit.useEffect();
                    }




                    ////////////////////Charge/////////////////////////
                   
                }

                Dice clickedDice = closestHit.collider.GetComponent<Dice>();
                if (clickedDice != null && clickedDice.isEnemy == false)
                {
                    selectedDice = clickedDice;
                }
            }
        }
    }

}
