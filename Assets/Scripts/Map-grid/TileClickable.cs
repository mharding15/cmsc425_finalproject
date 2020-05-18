using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClickable : MonoBehaviour
{

    public int tileX;
    public int tileY;
    public TileMap map;
    public int cost;


    void OnMouseDown(){
            Debug.Log("(" + tileX + " , " + tileY + ")");
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CombatLoop cl = GameObject.FindWithTag("Manager").GetComponent<CombatLoop>();
            cl.GetCurrentUnit().path = new pathFinder(map, tileX, tileY, new Vector3(tileX, tileY));
        }
    }
   
}
