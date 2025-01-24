using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _hieght;
    [SerializeField] private GameObject tile1;
    [SerializeField] private GameObject tile2;
    [SerializeField] private Camera MainCamera;
    private void Start()
    {
        GenerateGrid();
        SetCamera();
    }

    void GenerateGrid()
    {
        int tileNum = 0;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _hieght; y++)
            {
                //float xPos = x * 0.725f;                     
                //float yPos = (y + x) * 1 * 0.75f;        
                //var spawnedTile = Instantiate(tile, new Vector3(xPos, yPos, 0), Quaternion.Euler(60, 0, 45));
                tileNum ++;
                if (tileNum % 2 == 0)
                {
                    GameObject spawnedTile = Instantiate(tile1, new Vector2(x, y), quaternion.identity);
                    spawnedTile.name = $"Tile {x}, {y}";
                }
                else if (tileNum % 2 != 0)
                {
                    GameObject spawnedTile = Instantiate(tile2, new Vector2(x, y), quaternion.identity);
                    spawnedTile.name = $"Tile {x}, {y}";
                }
                
            }
        }
    }

    void SetCamera()
    {
        MainCamera.transform.position = new Vector3(_width/2, _hieght/2,-10);
    }
}
