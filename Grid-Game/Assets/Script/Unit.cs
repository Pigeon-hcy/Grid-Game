using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string Name;

    public bool selected;
    
    public GameManager gameManager;
 
    public NewUnit newUnit;
   

    public bool isEnemy;

    [Header("Moving")]
    public bool isMoving;
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
    public int maxHealth;
    public int attackRange;
    [SerializeField]
    public int attackDamage;

    [SerializeField]
    public List<Unit> nearByPlayer = new List<Unit>();

    [Header("PathFinding")]
    public Tile locateTile;
    private List<AStarNode> path = new List<AStarNode>();


    [Header("UnitEffect")]
    [SerializeField] List<ScriptableObject> listOfAbility = new List<ScriptableObject>();
    string effectName;
    public string EffectExplain;

    [Header("Animator")]
    [SerializeField]
    Animator animator;

    [Header("isenemy")]
    [SerializeField]
    GameObject enemy;
    [SerializeField]
    GameObject player;

    private void Start()
    {
        
        
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = newUnit.AnimatorController;
        locateTile.unitOnTile = true;
        turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(isEnemy == false)
            GetComponent<SpriteRenderer>().sprite = newUnit.Sprite;
        else
            GetComponent<SpriteRenderer>().sprite = newUnit.EnemySprite;
        Name = newUnit.UnitName;
        movement = newUnit.Movement;
        health = newUnit.Health;
        maxHealth = newUnit.Health;
        attackRange = newUnit.AttackRange;
        attackDamage = newUnit.Attack;
        effectName = newUnit.effectName;
        EffectExplain = newUnit.effectExplain;
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();

        if (isEnemy == true)
        {
            enemy.SetActive(true);
        }
        else if (isEnemy == false) {
        
            player.SetActive(true);
        }
    }

    private void Update()
    {
        if (isEnemy == false && health <= 0)
        {

                turnManager.PlayerList.Remove(this);
            

            Destroy(this.gameObject);
        }
    }

    public void useEffect()
    {
        switch (effectName)
        {
            case "selfHealing":
                (listOfAbility[0] as unitAbility)?.useEffect(this);
                turnManager.excuteTheBehave();
                Debug.Log("Self Heal");
                break;

            default:
                turnManager.excuteTheBehave();
                Debug.Log("No Skill");
                break;
        }
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
            turnManager.excuteTheBehave();
        }
        else
        {
            gridManager.ResetTile();
            return;
        }
    }


    public void ChargeTo(Tile target)
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
        }
        else
        {
            gridManager.ResetTile();
            return;
        }
    }

    private IEnumerator chargeAttack(Unit unit,Tile target)
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
        }
        else
        {
            gridManager.ResetTile();
        }
        yield return new WaitUntil(() => unit.isMoving == false);
        //attack
        Debug.Log("Charge R4A");
    }

    public void EnemyMoveTo(List<Unit> Players)
    {
        Tile player = findClosestPlayerUnit(Players);
        
        if (player != null)
        {
            List<Vector3> playerAroundTiles = findAroundingTiles(player);
            if (playerAroundTiles.Count > 0)
            {
                Vector3 bestTile = getCloseTile(playerAroundTiles);
                if (bestTile != null)
                {
                    path = gridManager.CalculatePathfinding(locateTile, gridManager.vectorToTile(bestTile), movement);
                    if (path != null && path.Count > 0)
                    {
                        StartCoroutine(followPath(path));
                    }
                }
            }
            else
            {
                Debug.Log("No Space");
            }
        }
        
        //Debug.Log("Enemymove2");
        //path = gridManager.CalculatePathfinding(locateTile, findClosestPlayerUnit(Players), movement);

        //if (path != null && path.Count > 0)
        //{
        //    Debug.Log("Enemymove3");
        //    //Tile nextPos = path[0].Tile;
        //    //if (!isEnemyOnIt(nextPos))
        //    //{
                
        //    //}
        //    //else
        //    //{
        //    //    Debug.Log("Block!");

        //    //}
        //    StartCoroutine(followPath(path));

        //}

    }


    public void Attack(Unit target)
    {
        target.health -= attackDamage;
        turnManager.excuteTheBehave();

        if (target.health <= 0)
        {
            if (target.isEnemy == true)
            {
                turnManager.EnemyList.Remove(target);
            }
            if (target.isEnemy == false)
            {
                turnManager.PlayerList.Remove(target);
            }

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


    public void drawAttackRange()
    {
        for (int x = -attackRange; x <= attackRange; x++)
        {
            for (int y = -attackRange; y <= attackRange; y++)
            {
                if (Mathf.Abs(y) + Mathf.Abs(x) > attackRange)
                {
                    continue;

                }
                else
                {
                    if (this.transform.position.x + x >= 0 && this.transform.position.x + x < gridManager.width && this.transform.position.y + y >= 0 && this.transform.position.y + y < gridManager.hieght)
                    {
                        Tile tile;
                        tile = gridManager.vectorToTile(new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0));
                        tile.drawAttack();
                    }
                }
            }
        }
    }
    



    private IEnumerator followPath(List<AStarNode> path)
    {
        for(int i = 0; i < path.Count; i++)
        {
            Tile nextPos = path[i].Tile;

            if (isEnemyOnIt(nextPos.transform.position) == true)
            {
                Debug.LogWarning("Block!");
                yield break;
            }

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

    private bool isEnemyOnIt(Vector3 tile)
    {
        foreach (Unit enemy in turnManager.EnemyList)
        {
            if (Vector2.Distance(tile, enemy.transform.position) < 0.9f)
            { 
                return true;
            }

        }
        return false;
    }

    private bool checkInRange(Vector3 playerPos)
    {
        return (playerPos.x < gridManager.width && playerPos.x >= 0 && playerPos.y < gridManager.hieght && playerPos.y >= 0);
    }



    private List<Vector3> findAroundingTiles(Tile tile)
    { 
        List<Vector3> opneTile = new List<Vector3>();
        Vector3[] directions =
        {
            tile.transform.position + new Vector3(1, 0, 0),
            tile.transform.position + new Vector3(-1, 0, 0),
            tile.transform.position + new Vector3(0, 1, 0),
            tile.transform.position + new Vector3(0, -1, 0),
        };

        foreach (Vector3 target in directions)
        {
            if (isEnemyOnIt(target) == false && checkInRange(target) && gridManager.vectorToTile(target).unitOnTile == false)
            {
                opneTile.Add(target);
            }
        }


        return opneTile;
    }


    private Vector3 getCloseTile(List<Vector3> targetList)
    {
        if (targetList.Count == 0)
        { 
            return this.transform.position;
        }
        float minDistance = int.MaxValue;
        Vector3 bestTile = targetList[0];

        foreach (Vector3 target in targetList)
        {
            float currentDistance = Vector2.Distance(bestTile, target);
            if (currentDistance < minDistance)
            {
                bestTile = target;
                minDistance = currentDistance;
            }

        }
        return bestTile;    
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
            path = gridManager.CalculatePathfinding(locateTile, players[i].locateTile, movement);

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
