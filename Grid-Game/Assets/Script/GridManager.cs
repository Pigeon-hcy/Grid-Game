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
    public GameObject[] Unit;

    public Tile[,] tiles;

    private void Start()
    {
        tiles = new Tile[width, hieght];
        GenerateGrid();
        SetCamera();
        placeUnit();
    }

    void placeUnit()
    {
        for (int i = 0; i < Unit.Length ; i++)
        {
            //0,0 is for debug only
            GameObject unit = Instantiate(Unit[i], new Vector2(0, 0),quaternion.identity);
            unit.GetComponent<Unit>().locateTile = tiles[0,0];
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
