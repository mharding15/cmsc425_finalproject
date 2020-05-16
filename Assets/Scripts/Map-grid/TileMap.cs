using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    public int mapSizeX = 10;
    public int mapSizeY = 10;
    int[,] tiles;
    public TileTerrain[] tileTypes;
    //Random r = new Random();
    public GameObject selectedUnit;


    void Start()
    {
        generateMap();
    }

    void generateMap()
    {
        //allocate tiles
        tiles = new int[mapSizeX, mapSizeY];

        //initialize tiles
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                int terrain = 0; //r.Next(0, 3); //TODO add randomization here!!
                tiles[x, y] = terrain;
                Instantiate(tileTypes[terrain].visualPrefab, new Vector3(x,0,y), Quaternion.identity);
            }
        }
    }
}
