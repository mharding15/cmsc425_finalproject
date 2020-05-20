using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform target;
    private Transform focus;
    private GameObject targetObject, focusObject;


    public Vector3 zoomVector;
    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 1f;
    public float maxZoom = 4f;

    public GameObject map;
    private TileMap tileMap;

    public float pitch = 3f;

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

    private bool freeCam = false;
    private bool effectiveFreeCam;
    private bool awaitingTarget = true;

    private Transform lockTarget = null;

    private void Start()
    {
        tileMap = map.GetComponent<TileMap>();
        targetObject = new GameObject();
        target = targetObject.transform;
        targetObject.name = "Free Camera Target";

        focusObject = new GameObject();
        focus = focusObject.transform;
        focusObject.name = "Free Camera Focus";

        startLerpPos = target.transform.position;
        lastZoom = currentZoom;
        startLerpZoom = currentZoom;

        
    }

    public void EnableFreeCam()
    {
        freeCam = true;
    }

    public void RemoveCameraLock()
    {
        lockTarget = null;
        awaitingTarget = true;
    }

    // LERPs to a transform. Camera follows that transform until freecam is enabled
    public void LerpToTransformAndLock(Transform transf)
    {
        if (transf != null)
        {
            startLerpPos = focus.position;
            target.position = transf.position;
            lockTarget = transf;
            lerpStartTime = Time.time;
            freeCam = false;
            awaitingTarget = false;
        }
        
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

    public void setCameraLockable(Transform target)
    {
        if (freeCam)
        {
            lockTarget = target;
        }
        else
        {
            LerpToTransformAndLock(target);
        }
    }

    public void setCameraLockableObject(GameObject target)
    {
        setCameraLockable(target.transform);
    }

    void Update()
    {
        focus.transform.position = Vector3.Lerp(startLerpPos, target.position, 
            Mathf.Pow(Time.time - lerpStartTime, 0.4f)/ cameraLerpTime );

        // 
        if (Input.GetMouseButtonDown(2))
        {
            if (freeCam)
            {
                if (lockTarget != null)
                {
                    LerpToTransformAndLock(lockTarget);
                }
            }
            else
            {
                EnableFreeCam();
            }
        }

        if (awaitingTarget && !freeCam)
        {
            effectiveFreeCam = true;
        }
        else
        {
            effectiveFreeCam = freeCam;
        }

        // Emergency Enable free cam
        if (Input.GetKeyDown(KeyCode.L))
        {
            EnableFreeCam();
        }

        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        float multiplier;
        if (Input.GetKey(KeyCode.LeftShift))
            multiplier = 2;
        else
            multiplier = 1;

        if (Input.GetKey(KeyCode.Q))
            currentYaw -= yawSpeed * Time.deltaTime * multiplier;

        if (Input.GetKey(KeyCode.E))
            currentYaw += yawSpeed * Time.deltaTime * multiplier;

        
        
        deltaHorizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * multiplier;
        deltaVertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * multiplier;

        
    }

    void LateUpdate()
    {

        Quaternion camDir = Quaternion.Euler(0, currentYaw, 0);
        
        Debug.DrawRay(target.position, camDir * Vector3.forward);

        if (effectiveFreeCam)
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
