using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected;
    
    public GameManager gameManager;
 
    public NewUnit newUnit;
   

    public bool isEnemy;

    [Header("Moving")]
    bool isMoving;
    [SerializeField]
    float timeToMove;
    public int movement;
    [SerializeField]
    GridManager gridManager;
    [SerializeField]
    TurnManager turnManager;

    [Header("Combat")]
    [SerializeField]
    public int health;
    public int attackRange;
    [SerializeField]
    public int attackDamage;

    [SerializeField]
    public List<Unit> nearByPlayer = new List<Unit>();

    [Header("PathFinding")]
    public Tile locateTile;
    private List<AStarNode> path = new List<AStarNode>();





    private void Start()
    {
        locateTile.unitOnTile = true;
        turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(isEnemy == false)
            GetComponent<SpriteRenderer>().sprite = newUnit.Sprite;
        else
            GetComponent<SpriteRenderer>().sprite = newUnit.EnemySprite;
        movement = newUnit.Movement;
        health = newUnit.Health;
        attackRange = newUnit.AttackRange;
        attackDamage = newUnit.Attack;
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
    }

    public void MoveTo(Tile target)
    {
        if (target.unitOnTile == false && target.available == true && isEnemy == false && isMoving == false)
        {
            locateTile.unitOnTile = false;
            locateTile = target;
            locateTile.playerOnIt = false;
            //move the unit
            target.unitOnTile = true;
            locateTile.playerOnIt = true;
            StartCoroutine(MovePlayer(target.transform.position));
            gridManager.ResetTile();
            Debug.Log("can move");
            turnManager.excuteTheBehave();
        }
        else
        {
            gridManager.ResetTile();
            Debug.Log("can't move");
            return;
        }
    }


    public void EnemyMoveTo(List<Unit> Players)
    {
        Debug.Log("Enemymove2");
        path = gridManager.CalculatePathfinding(locateTile, findClosestPlayerUnit(Players), movement);
        if (path != null && path.Count > 0)
        {
            Debug.Log("Enemymove3");
            StartCoroutine(followPath(path));
            
        }

    }


    public void Attack(Unit target)
    {
        target.health -= attackDamage;
        turnManager.excuteTheBehave();

        if (target.health <= 0)
        { 
            Destroy(target.gameObject);
        }

    }



    public bool CheckForEnemies(int attackRange)
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
                if (nearbyUnit != null && nearbyUnit.isEnemy /*&& turnManager.currentBehave == "Attack"*/) // Found an enemy nearby
                {
                    Debug.Log("Can attack!");
                    return true;
                }
            }
        }
        return false;
    }

    //private void OnMouseDown()
    //{
    //    gameManager.selectedUnit = this;
    //}

    public bool CheckForPlayer(int attackRange)
    {
        nearByPlayer.Clear();
        List<Vector3> attackOffsets = new List<Vector3>();

        for (int x = -attackRange; x <= attackRange; x++)
        {
            for (int y = -attackRange; y <= attackRange; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= attackRange && !(x == 0 && y == 0))
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
            Collider2D hit = Physics2D.OverlapBox(checkPos, boxSize, 0);
            Debug.Log(hit);
            if (hit != null)
            {
                
                Unit nearbyUnit = hit.GetComponent<Unit>();
                
                if (nearbyUnit != null && !nearbyUnit.isEnemy  /*&& turnManager.currentBehave == "Attack"*/) // Found an player nearby
                {
                    
                    nearByPlayer.Add(nearbyUnit);
                    return true;
                }
            }
        }
        
        return false;
    }



    



    private IEnumerator followPath(List<AStarNode> path)
    {
        for(int i = 0; i < path.Count - 1; i++)
        {
            locateTile.unitOnTile = false;
            locateTile = path[i].Tile;
            locateTile.unitOnTile = true;

            isMoving = true;

            Vector3 originPos = transform.position;

            float elapsedTime = 0;

            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(originPos, path[i].Tile.transform.position, elapsedTime / timeToMove);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = path[i].Tile.transform.position;
            //CheckForEnemies(attackRange);
            isMoving = false;


            //Debug.Log(path[i].GCost);
            yield return new WaitForSeconds(0.3f);
        }
    }
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

        //CheckForEnemies(attackRange);
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

    private Tile findClosestPlayerUnit(List<Unit> players)
    { 
        Unit nearest = null;
        int shortest = int.MaxValue;
        Debug.Log("Enemymove4");
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                continue;
            }
            path = gridManager.CalculatePathfinding(locateTile, players[i].locateTile, int.MaxValue);
            if (path != null && path.Count > 0 && path.Count < shortest)
            {
                shortest = path.Count;
                nearest = players[i];
            }
        }

        return nearest.locateTile;
    }


    public void reLocate()
    { 
        locateTile.playerOnIt = true;
    }
}
