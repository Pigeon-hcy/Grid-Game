using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.TextCore.Text;
public class AStarNode
{
    public Tile Tile;
    public AStarNode Parent;
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;
}


public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _hieght;
    //width and hight need larger than 2
    public int width
        { get { return _width; } set { _width = Mathf.Max(_width,2); } }
    public int hieght
    { get { return _hieght; } set { _hieght = Mathf.Max(_hieght, 2); } }


    [SerializeField] private GameObject tile1;
    [SerializeField] private GameObject tile2;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private GameObject Unit;
    public NewUnit[] PlayerUnit;
    public NewUnit[] EnemyUnit;
    public Tile[,] tiles;
    [HideInInspector]
    public Tile currentAvailableMoveTarget;
    private void Start()
    {
        tiles = new Tile[width, hieght];
        GenerateGrid();
        SetCamera();
        placeUnit();
        placeEnemy();
    }

    void placeUnit()
    {
        for (int i = 0; i < PlayerUnit.Length ; i++)
        {
            //0,0 is for debug only
            GameObject unit = Instantiate(Unit, new Vector2(0, i),quaternion.identity);
            unit.GetComponent<Unit>().newUnit = PlayerUnit[i];
            unit.GetComponent<Unit>().locateTile = tiles[0,i];
        }
    }


    void placeEnemy()
    {
        for (int i = 0; i < EnemyUnit.Length; i++)
        {
            //0,0 is for debug only
            GameObject unit = Instantiate(Unit, new Vector2(_width - 1, i), quaternion.identity);
            unit.GetComponent<Unit>().newUnit = EnemyUnit[i];
            unit.GetComponent<Unit>().locateTile = tiles[_width - 1, i];
            unit.GetComponent<Unit>().isEnemy = true;
        }
    }

    void GenerateGrid()
    {
        int tileNum = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hieght; y++)
            {
                //float xPos = x * 0.725f;                     
                //float yPos = (y + x) * 1 * 0.75f;        
                //var spawnedTile = Instantiate(tile, new Vector3(xPos, yPos, 0), Quaternion.Euler(60, 0, 45));
                tileNum ++;
                
                bool tileIndex = tileNum % 2 == 0 ? true : false;
                if (tileIndex)
                {
                    GameObject spawnedTile = Instantiate(tile1, new Vector2(x, y), quaternion.identity);
                    spawnedTile.GetComponent<Tile>().x = x;
                    spawnedTile.GetComponent<Tile>().y = y;
                    spawnedTile.name = $"Tile {x}, {y}";
                    tiles[x, y] = spawnedTile.GetComponent<Tile>();
                }
                else
                {
                    GameObject spawnedTile = Instantiate(tile2, new Vector2(x, y), quaternion.identity);
                    spawnedTile.GetComponent<Tile>().x = x;
                    spawnedTile.GetComponent<Tile>().y = y;
                    spawnedTile.name = $"Tile {x}, {y}";
                    tiles[x, y] = spawnedTile.GetComponent<Tile>();
                }

                
            }
        }
    }

    void SetCamera()
    {
        MainCamera.transform.position = new Vector3(_width/2, _hieght/2,-10);
    }

    //----------------------------------------------------------------------//


    //get neighbour tiles
    private List<Tile> GetNeighbors(Tile gridUnit)
    {
        //Find the where the gridUnit is
        List<Tile> neighbors = new List<Tile>();
        int mapX = -1;
        int mapY = -1;
        //Go through tiles and find gridUnit
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < hieght; y++)
            {
                if (tiles[x, y] == gridUnit)
                {
                    mapX = x;
                    mapY = y;
                    break;
                }
            }
        }
        //if not return empty
        if (mapX == -1 || mapY == -1)
            return neighbors;
        //Four direction
        int[,] directions = new int[,] 
        { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
        //put four direction into list
        for (int i = 0; i < 4; i++)
        {
            int newX = mapX + directions[i, 0];
            int newY = mapY + directions[i, 1];
            //prevent adding -1 or larger than wid and hieght
            if (newX >= 0 && newX < width && newY >= 0 && newY < hieght)
            {
                neighbors.Add(tiles[newX, newY]);
            }
        }

        return neighbors;
    }


    //When click on the character, calculate the range for the reachable area
    public List<Tile> CalculateReachableGrids(Tile startLocation, int moveability)
    {
        List<Tile> reachableGrids = new List<Tile>();
        //First in first out
        Queue<(Tile, int)> queue = new Queue<(Tile, int)>();
        HashSet<Tile> visited = new HashSet<Tile>();
        queue.Enqueue((startLocation, 0));
        visited.Add(startLocation);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            Tile gridUnit = current.Item1; //The locate of the tile
            int cost = current.Item2; //The cost of tile if u move to there

            if (cost <= moveability)
            {
                reachableGrids.Add(gridUnit);

                foreach (Tile neighbor in GetNeighbors(gridUnit))
                {
                    int newCost = cost + GetCost(neighbor.tileType);

                    //Add neighbor tiles into the queue, as long as: 1. It's not visited; 2.Overall cost is lower than actionPoint; 
                    if (!visited.Contains(neighbor) && newCost <= moveability)
                    {
                        queue.Enqueue((neighbor, newCost));
                        visited.Add(neighbor);
                    }
                }
            }
        }

        return reachableGrids;
    }


    //Pathing finding using A*
    public void CalculatePathfinding(Tile startLocation, Tile targetLocation, int moveability)
    {
        // Create the open list containing the start node and closed set
        List<AStarNode> openList = new List<AStarNode>();
        HashSet<Tile> closedSet = new HashSet<Tile>();
        Dictionary<Tile, AStarNode> nodeLookup = new Dictionary<Tile, AStarNode>();
        AStarNode startNode = new AStarNode { Tile = startLocation };
        openList.Add(startNode);
        nodeLookup[startLocation] = startNode;

        while (openList.Count > 0)
        {
            // Find the node with the lowest F cost
            AStarNode currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                

                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost)
                {
                    currentNode = openList[i];
                }
            }

            // Remove the current node from the open list and add it to the closed set
            openList.Remove(currentNode);
            closedSet.Add(currentNode.Tile);

            // Check if the current node is the target location, if so, retrace the path
            if (currentNode.Tile == targetLocation)
            {
                //RetracePath(startNode, currentNode, moveability);
                return;
            }

            // Explore neighbors of the current node
            foreach (Tile neighbor in GetNeighbors(currentNode.Tile))
            {
                // Ignore blocked nodes or nodes already in the closed set
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }
                // Calculate the new G cost
                int newGCost = currentNode.GCost + GetDistance(currentNode.Tile, neighbor);
                AStarNode neighborNode;
                if (!nodeLookup.ContainsKey(neighbor))
                {
                    neighborNode = new AStarNode { Tile = neighbor };
                    nodeLookup[neighbor] = neighborNode;
                }
                else
                {
                    neighborNode = nodeLookup[neighbor];
                }
                // Update neighbor's G, H, and Parent if the new G cost is lower, or if the neighbor is not in the open list
                if (newGCost < neighborNode.GCost || !openList.Contains(neighborNode))
                {
                    neighborNode.GCost = newGCost;
                    neighborNode.HCost = GetDistance(neighbor, targetLocation);
                    neighborNode.Parent = currentNode;
                    // Add neighbor to the open list if it's not already there
                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
    }

    //get the manhadon distance of 2 tiles
    private int GetDistance(Tile a, Tile b)
    {
        //Use manhattan distance formula to calculate H cost (heuristic cost)
        int distX = Mathf.Abs(a.x - b.x);
        int distY = Mathf.Abs(a.y - b.y);

        if (distX > distY)
        {
            return distX;
        }
        return distY;
    }


    public void ShowTheMovingRange(Unit character)
    {
        List<Tile> reachableGrids = CalculateReachableGrids(character.locateTile, character.movement);
        foreach (Tile gridUnit in reachableGrids)
        {
            gridUnit.highLight();
        }
    }

    //Show the where is the avaliable block
    //private void RetracePath(AStarNode startNode, AStarNode endNode, int actionPoint)
    //{
    //    // Create an empty list to store the path
    //    List<Tile> path = new List<Tile>();
    //    AStarNode currentNode = endNode;
    //    // Follow parent pointers from the end node to the start node, adding each node to the path
    //    while (currentNode != startNode)
    //    {
    //        path.Add(currentNode.Tile);
    //        currentNode = currentNode.Parent;
    //    }
    //    // Reverse the path so it starts from the starting location
    //    path.Reverse();
    //    // Hide the pathfinding visualization for all grid units
    //    foreach (Tile gridUnit in gridUnitList)
    //    {
    //        gridUnit.HidePathfinding();
    //    }
    //    // Display the path and calculate its cost
    //    int pathCost = 0;
    //    foreach (Tile gridUnit in path)
    //    {
    //        pathCost += 1;
    //        // If the path cost exceeds the action points available, stop displaying the path
    //        if (pathCost > actionPoint)
    //        {
    //            break;
    //        }
    //        // Update the current available move target and display the pathfinding visualization with the path cost
    //        currentAvailableMoveTarget = gridUnit;
    //        gridUnit.DisplayPathfinding(pathCost.ToString());
    //    }
    //}

    int GetCost(int tileType)
    {
        int cost = 0;
        switch (tileType)
        {
            case 0:
                //Grass
                cost = 1;
                break;
            case 1:
                //Rock, no way to pass. Worked for blocked nodes
                cost = 99;
                break;
        }

        return cost;
    }
}
