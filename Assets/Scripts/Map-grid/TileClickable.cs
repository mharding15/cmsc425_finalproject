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
            //CombatLoop cl = GameObject.FindWithTag("Manager").GetComponent<CombatLoop>();
            //cl.GetCurrentUnit.path =

            otherCall();
        } else
        {
            vate float _lastUpdateTime = 0f;

            void Update()
            {
                if (Time.time - _lastUpdateTime >= 1.0f)
                {
                    _lastUpdateTime = Time.time;
                    // TODO: Update your value
                }
            }
            //map.selectedUnit.MoveTo(3, 3);
        }

        
    }

    public bool isWalkable()
    {
        return (cost > 0);
    }

    public void otherCall()
    {
        List<Vector3> directions = (new pathFinder(map, 3, 3, this)).solve();
        for (int i = 0; i < directions.Count; i++)
        {
            map.selectedUnit.MoveTo((int)directions[i].x, (int)directions[i].y);
            float _lastUpdateTime = 0f;

            while (Time.time - _lastUpdateTime < 1.0f)
            {
                _lastUpdateTime = Time.time;
                    
            }
            
        }
    }

    
}
