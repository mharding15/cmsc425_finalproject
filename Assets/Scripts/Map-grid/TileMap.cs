using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    public int mapSizeX = 10;
    public int mapSizeY = 10;
    int[,] tiles;

    public TileTerrain[] tileTypes;
    public Unit selectedUnit;


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
                //hard codng map tiles
                int terrain = 0; 

                if ( (x == 7) && ( y > 2 && y < 6) ) {
                    terrain = 1;
                } else if( y == 2){
                    terrain = 2;
                }

                //let the map know your type
                tiles[x, y] = terrain;
                GameObject thisTile = (GameObject) Instantiate(tileTypes[terrain].visualPrefab, new Vector3(x,0,y), Quaternion.identity);

                //gnothi seaton
                TileClickable clickPos = thisTile.GetComponent<TileClickable>();
                clickPos.tileX = x;
                clickPos.tileY = y;
                clickPos.map = this;
                clickPos.cost = tileTypes[terrain].moveCost;
            }
        }
    }

}
