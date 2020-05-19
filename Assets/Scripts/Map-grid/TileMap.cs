using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TileMap : MonoBehaviour
{

    public int mapSizeX = 25;
    public int mapSizeY = 25;
    Mesh grid;
    public MeshFilter filter;
    public Vector3 offset;

    Vector3[] verts;
    int[] triangles;


    void Start()
    {
        grid = new Mesh();
        GetComponent<MeshFilter>().mesh = grid;
        offset = new Vector3(2, 2, 2); //(GameObject.FindWithTag("Terrain").transform.position/5); 
        generateMap();
    }

    void Update()
    {
        UpdateMap();
    }

    void generateMap()
    {
        verts = new Vector3[(mapSizeX + 1) * (mapSizeY + 1)];
        for (int i =0, y = 0; y <= mapSizeY; y++)
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                verts[i] = new Vector3(x, 0, y) + offset;
                i++;
            }
        }

        triangles = new int[mapSizeX * mapSizeY * 6];

        int vertCount = 0;
        int triCount = 0;

        for (int y = 0; y < mapSizeY; y++)
        {
            for (int x = 0; x < mapSizeX; x++)
            {
                triangles[triCount + 0] = vertCount + 0;
                triangles[triCount + 1] = vertCount + mapSizeX + 1;
                triangles[triCount + 2] = vertCount + 1;
                triangles[triCount + 3] = vertCount + 1;
                triangles[triCount + 4] = vertCount + mapSizeX + 1;
                triangles[triCount + 5] = vertCount + mapSizeX + 2;

                vertCount++;
                triCount += 6;
            }
            vertCount++; //next tile
        }
    }



    void UpdateMap()
    {
        grid.Clear();

        grid.vertices = verts;
        grid.triangles = triangles;

        grid.RecalculateNormals();
    }

}
