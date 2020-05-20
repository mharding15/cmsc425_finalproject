using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    //used for global variables and persistant behavior
    
    public static Manager Instance { get; private set; }

    
    public Camera mainCamera;
    public GameObject tileSelectionIndicator;
    public LayerMask tileMask;
    public GameObject movableTileIndicatorPrefab, selectedUnitIndicator, seletedTargetIndicator, enemyUnitIndicator, friendlyUnitIndicator;
    public GameObject attackableTilePrefab, attackableTileGhostPrefab;
    public LineRenderer unitPath;

    public TileMap map;

    public List<Vector3> currentPath = null;

    private GameObject lastHoveredTile = null;

    private List<GameObject> movableTiles = new List<GameObject>(), attackableTiles = new List<GameObject>();

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
            lastHoveredTile = currentHoveredTile;

            /*for (int i = 0; i < movableTiles.Count; i++)
                Destroy(movableTiles[i]);
            movableTiles.Clear();
            */


            if (currentHoveredTile != null)
            {
                tileSelectionIndicator.transform.position = currentHoveredTile.transform.position;
                if (GetComponent<CombatLoop>().GetUnits().Count > 0 && GetComponent<CombatLoop>().getCurrentInt() >= 0)
                {
                    Unit curUnit = GetComponent<CombatLoop>().GetCurrentUnit();
                    if (curUnit != null)
                    {
                        if (!curUnit.GetHasActed())
                        {
                            if (curUnit.movableTiles.Contains(currentHoveredTile.transform.position) )
                            {
                            
                                List<Vector3> path = (new pathFinder(map, Mathf.RoundToInt(curUnit.transform.position.x), Mathf.RoundToInt(curUnit.transform.position.z),
                                    (int)currentHoveredTile.transform.position.x, (int)currentHoveredTile.transform.position.z)).solve();
                                List<Vector3> newPath = new List<Vector3>();
                                for (int i = 0; i < path.Count; i++)
                                    newPath.Add(path[i] + Vector3.up);

                                unitPath.positionCount = newPath.Count;
                                unitPath.SetPositions(newPath.ToArray());
                            }
                            else
                            {
                                unitPath.positionCount = 0;
                            }
                        }
                    }
                    else
                    {
                        unitPath.positionCount = 0;
                    }
                }
                else
                {
                    unitPath.positionCount = 0;
                }
                

                //MovableTileFinder mov = new MovableTileFinder(map, (int)(currentHoveredTile.transform.position.x), (int)(currentHoveredTile.transform.position.z), 23);
                //List<Vector3> movableLocations = mov.solve();
                //SetMovableTilePreview(movableLocations);
            }
            else
            {

                unitPath.positionCount = 0;
                tileSelectionIndicator.transform.position = garbagePosition;
            }

            
        }
    }

    public void ResetTargetIndicator()
    {
        seletedTargetIndicator.transform.position = garbagePosition;
    }

    public void SetTargetIndicator(Vector3 pos)
    {
        seletedTargetIndicator.transform.position = pos;
    }

    public void SetAttackableTilePreview(Unit unit)
    {
        for (int i = 0; i < attackableTiles.Count; i++)
            Destroy(attackableTiles[i]);
        attackableTiles.Clear();

        foreach (Unit u in Manager.Instance.GetComponent<CombatLoop>().GetUnits())
        {
            if (unit.isEnemy != u.isEnemy && u.hp > 0)
            {
                if (Vector3.Distance(unit.transform.position, u.transform.position) <= unit.meleeRange)
                    attackableTiles.Add(Instantiate(attackableTilePrefab, u.transform.position, Quaternion.identity) as GameObject);
                else if (Vector3.Distance(unit.transform.position, u.transform.position) <= unit.longRange)
                    attackableTiles.Add(Instantiate(attackableTileGhostPrefab, u.transform.position, Quaternion.identity) as GameObject);
            }
        }
    }

    public void SetMovableTilePreview(List<Vector3> tiles)
    {
        for (int i = 0; i < movableTiles.Count; i++)
            Destroy(movableTiles[i]);
        movableTiles.Clear();

        if (tiles != null)
        {
            for (int i = 0; i < tiles.Count; i++)
                movableTiles.Add(Instantiate(movableTileIndicatorPrefab, tiles[i], Quaternion.identity) as GameObject);
        }
    }

    public void SetSelectedUnitIndicator(Transform transf)
    {
        selectedUnitIndicator.transform.position = transf.position;
        selectedUnitIndicator.transform.parent = transf;
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
