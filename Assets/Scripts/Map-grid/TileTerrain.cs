using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileTerrain
{

    public enum material : short
    {
        GRASS, DIRT, WATER
    };

    public material terrType;
    public int moveCost;
    public GameObject visualPrefab;
    public bool occupied = false;

    public int cost()
    {
        return moveCost;
    }

}
