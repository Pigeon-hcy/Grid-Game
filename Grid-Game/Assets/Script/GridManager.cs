using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
                    spawnedTile.name = $"Tile {x}, {y}";
                    tiles[x, y] = spawnedTile.GetComponent<Tile>();
                }
                else
                {
                    GameObject spawnedTile = Instantiate(tile2, new Vector2(x, y), quaternion.identity);
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
}
