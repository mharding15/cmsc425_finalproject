using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClickable : MonoBehaviour
{

    public int tileX;
    public int tileY;
    public TileMap map;

    private void printName(GameObject go)
    {
        Debug.Log(go.name);
    }

    void OnMouseDown(){
            Debug.Log("(" + tileX + " , " + tileY + ")");
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            map.moveUnitTo(tileX, tileY);
        }
    }
   
}
