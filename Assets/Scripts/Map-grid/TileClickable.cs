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
            GameObject.FindWithTag("Manager").GetComponent<Manager>().currentPath = (new pathFinder(map, 3, 3, tileX, tileY)).solve();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            print("right click-----------------------------------------------------------------------------------------------------------");
            CombatLoop cl = GameObject.FindWithTag("Manager").GetComponent<CombatLoop>();
            cl.GetCurrentUnit().path = (new pathFinder(map, 3, 3, tileX, tileY)).solve();
            print("unit given path: "+cl.GetCurrentUnit().path);
            cl.GetCurrentUnit().EnterMoveMode();
        }
    }   
}
