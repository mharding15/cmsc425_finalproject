using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public Transform focus;
    public Vector3 zoomVector;
    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 1f;
    public float maxZoom = 7f;

    public GameObject map;
    private TileMap tileMap;

    public float pitch = 0.2f;

    public float yawSpeed = 100f;
    public float moveSpeed = 10f;
    public LayerMask tileMask;

    private float currentZoom = 2.5f;
    private float lastZoom;
    private float currentYaw = 0f;

    private float deltaVertical = 0f;
    private float deltaHorizontal = 0f;
    private float lerpStartTime = 0f;
    private float zoomLerpStartTime = 0f;
    private float startLerpZoom;

    private Vector3 startLerpPos;

    private float cameraLerpTime = 0.7f;
    private float zoomLerpTime = 0.3f;

    private bool freeCam = true;

    private Transform lockTarget = null;

    private void Start()
    {
        tileMap = map.GetComponent<TileMap>();
        startLerpPos = target.transform.position;
        lastZoom = currentZoom;
        startLerpZoom = currentZoom;
    }

    public void EnableFreeCam()
    {
        freeCam = true;
    }

    // LERPs to a transform. Camera follows that transform until freecam is enabled
    public void LerpToTransformAndLock(Transform transf)
    {
        startLerpPos = focus.position;
        target.position = transf.position;
        lockTarget = transf;
        lerpStartTime = Time.time;
        freeCam = false;
    }

    public void LerpToObjectAndLock(GameObject obj)
    {
        LerpToTransformAndLock(obj.transform);
    }

    // LERP to a position. Freecam will be enabled
    public void LerpToPos(Vector3 pos)
    {
        startLerpPos = focus.position;
        target.position = pos;
        lerpStartTime = Time.time;
        freeCam = true;
    }
    void Update()
    {
        focus.transform.position = Vector3.Lerp(startLerpPos, target.position, 
            Mathf.Pow(Time.time - lerpStartTime, 0.4f)/ cameraLerpTime );


        // Emergency Enable free cam
        if (Input.GetKeyDown(KeyCode.L))
        {
            EnableFreeCam();
        }

            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);


        if (Input.GetKey(KeyCode.Q))
            currentYaw -= yawSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
            currentYaw += yawSpeed * Time.deltaTime;

        
        deltaHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        deltaVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        
    }

    void LateUpdate()
    {

        Quaternion camDir = Quaternion.Euler(0, currentYaw, 0);
        
        Debug.DrawRay(target.position, camDir * Vector3.forward);

        if (freeCam)
        {
            target.Translate(camDir * Vector3.forward * deltaVertical + camDir * Vector3.right * deltaHorizontal);
            Vector3 targetPos = target.transform.position;
            // Make sure we're in the Map Area
            target.transform.position = new Vector3(Mathf.Clamp(targetPos.x, 0, tileMap.mapSizeX - 1),
            targetPos.y,
            Mathf.Clamp(targetPos.z, 0, tileMap.mapSizeY - 1)
            );
        }
        else
        {
            target.position = lockTarget.position;
        }
        
        
        
        if (currentZoom != lastZoom)
        {
            startLerpZoom = Mathf.Lerp(startLerpZoom, lastZoom, Mathf.Pow(Time.time - zoomLerpStartTime, 0.5f) / zoomLerpTime);
            lastZoom = currentZoom;
            zoomLerpStartTime = Time.time;
        }


        transform.position = focus.position - zoomVector * Mathf.Lerp(startLerpZoom, currentZoom,
            Mathf.Pow(Time.time - zoomLerpStartTime, 0.5f) / zoomLerpTime) + offset;
        transform.LookAt(focus.position + Vector3.up * pitch);
        transform.RotateAround(focus.position, Vector3.up, currentYaw);

    }
}
