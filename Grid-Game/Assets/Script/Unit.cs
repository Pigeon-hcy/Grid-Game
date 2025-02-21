using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected;
    public Tile locateTile;
    public GameManager gameManager;
 
    public NewUnit newUnit;
    public int movement;
    int health;
    int attackRange;
    public bool isEnemy;

    [Header("Moving")]
    bool isMoving;
    [SerializeField]
    float timeToMove;

    [SerializeField]
    GridManager gridManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(isEnemy == false)
            GetComponent<SpriteRenderer>().sprite = newUnit.Sprite;
        else
            GetComponent<SpriteRenderer>().sprite = newUnit.EnemySprite;
        movement = newUnit.Movement;
        health = newUnit.Health;
        attackRange = newUnit.AttackRange;
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
    }

    public void MoveTo(Tile target)
    {
        if (target.unitOnTile == false && target.available == true && isEnemy == false && isMoving == false)
        {
            locateTile.unitOnTile = false;
            locateTile = target;
            //move the unit
            target.unitOnTile = true;
            StartCoroutine(MovePlayer(target.transform.position));
            gridManager.ResetTile();
            Debug.Log("can move");
        }
        else
        {
            gridManager.ResetTile();
            Debug.Log("can't move");
            return;
        }


    }


    private void CheckForEnemies(int attackRange)
    {
        List<Vector3> attackOffsets = new List<Vector3>();

        for (int x = -attackRange; x <= attackRange; x++)
        {
            for (int y = -attackRange; y <= attackRange; y++)
            {
                if(Mathf.Abs(x) + Mathf.Abs(y) <= attackRange && !(x == 0 && y == 0))
                {
                    attackOffsets.Add(new Vector3(x, y, 0));
                }
            }
        }
        Vector3[] attackOffsetsArray = attackOffsets.ToArray();

        foreach (Vector3 offset in attackOffsets)
        {
            Vector3 checkPos = transform.position + offset;
            //Collider2D hit = Physics2D.OverlapCircle(checkPos, 0.1f); // Check for unit presence
            Vector2 boxSize = new Vector2(0.9f, 0.9f); // Adjust based on the true tile size, for now just assume grid 1 *1
            Collider2D hit = Physics2D.OverlapBox(checkPos, boxSize, 0f);

            if (hit != null)
            {
                Unit nearbyUnit = hit.GetComponent<Unit>();
                if (nearbyUnit != null && nearbyUnit.isEnemy) // Found an enemy nearby
                {
                    Debug.Log("Can attack!");
                    return;
                }
            }
        }
    }

    //private void OnMouseDown()
    //{
    //    gameManager.selectedUnit = this;
    //}
    private IEnumerator MovePlayer(Vector3 targetPos)
    {
        isMoving = true;

        Vector3 originPos = transform.position;

        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        CheckForEnemies(attackRange);
        isMoving = false;
    }


    private void OnDrawGizmosSelected()
    {
        //Debug.Log("Drawing Gizmos...");
        Gizmos.color = Color.red; // Highlight attack range when selected
        for (int x = -attackRange; x <= attackRange; x++)
        {
            for (int y = -attackRange; y <= attackRange; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= attackRange && !(x == 0 && y == 0))
                {
                    Vector3 checkPos = transform.position + new Vector3(x, y, 0);
                    Gizmos.DrawWireCube(checkPos, new Vector3(1, 1, 0)); 
                }
            }
        }

    }
}
