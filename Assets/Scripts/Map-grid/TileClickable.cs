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
            //Debug.Log("(" + tileX + " , " + tileY + ")");
            GameObject.FindWithTag("Manager").GetComponent<Manager>().currentPath = (new pathFinder(map, 9, 2, tileX, tileY)).solve();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            print("right click-----------------------------------------------------------------------------------------------------------");
            CombatLoop cl = GameObject.FindWithTag("Manager").GetComponent<CombatLoop>();
            cl.GetCurrentUnit().PathTo(tileX,tileY);
        }
    }   
}
