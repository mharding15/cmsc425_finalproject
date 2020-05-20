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
    public GameObject movableTileIndicatorPrefab;

    private TileMap map;

    public List<Vector3> currentPath = null;

    private GameObject lastHoveredTile = null;

    private List<GameObject> movableTiles = new List<GameObject>();

    private GameObject currentHoveredTile;

    private Vector3 garbagePosition = new Vector3(0, 100, 0);

    private void Start()
    {
        map = GameObject.FindWithTag("Map").GetComponent<TileMap>();
    }

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
            currentHoveredTile = hit.collider.gameObject;
        } else
        {
            currentHoveredTile = null;
        }

        if (currentHoveredTile != lastHoveredTile) // If hovered tile changed.
        {
            for (int i = 0; i < movableTiles.Count; i++)
                Destroy(movableTiles[i]);
            movableTiles.Clear();

            if (currentHoveredTile != null)
            {
                tileSelectionIndicator.transform.position = currentHoveredTile.transform.position;
                

                List<Vector3> movableLocations = (new MovableTileFinder(map, (int)(currentHoveredTile.transform.position.x), (int)(currentHoveredTile.transform.position.z), 40)).solve();

                for (int i = 0; i < movableLocations.Count; i++)
                    movableTiles.Add(Instantiate(movableTileIndicatorPrefab, movableLocations[i], Quaternion.identity) as GameObject);
            }
            else
            {
                tileSelectionIndicator.transform.position = garbagePosition;
            }

            lastHoveredTile = currentHoveredTile;
        }
    }

    private void LateUpdate()
    {

        if (currentPath != null) { 
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Debug.DrawRay(currentPath[i] + Vector3.up, currentPath[i+1] - currentPath[i]);
            }
        }


    }
}
