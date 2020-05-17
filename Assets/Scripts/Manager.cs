using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    //used for global variables and persistant behavior
    
    public static Manager Instance { get; private set; }

    public int mapSizeX = 10;
    public int mapSizeY = 10;
    public Camera mainCamera;
    public GameObject tileSelectionIndicator;
    public LayerMask tileMask;

    private GameObject currentTile;

    private Vector3 garbagePosition = new Vector3(0, 100, 0);

    //makes sure there is only one Manager instance at any time 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

        // Tile Selection
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200, tileMask)) // If mouse is over a tile...
        {
            currentTile = hit.collider.gameObject;
            // Move the tile indicator to our desired space
            tileSelectionIndicator.transform.position = currentTile.transform.position;
        } else
        {
            currentTile = null;
            // Move the tile indicator to our desired space
            tileSelectionIndicator.transform.position = garbagePosition;
        }
    }
}
