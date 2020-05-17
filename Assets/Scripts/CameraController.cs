﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 1f;
    public float maxZoom = 5f;
    public Manager manager;

    public float pitch = 1;

    public float yawSpeed = 100f;
    public float moveSpeed = 10f;

    private float currentZoom = 2.5f;
    private float currentYaw = 0f;

    private float deltaVertical = 0f;
    private float deltaHorizontal = 0f;


    void Update()
    {
        

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
        //target.transform.Translate(Vector3.forward * deltaVertical + Vector3.right * deltaHorizontal);
        target.transform.Translate(camDir * Vector3.forward * deltaVertical + camDir * Vector3.right * deltaHorizontal);
        
        Vector3 targetPos = target.transform.position;
        
        // Make sure we're in the Map Area
        target.transform.position = new Vector3(Mathf.Clamp(targetPos.x, 0, manager.mapSizeX - 1),
            targetPos.y,
            Mathf.Clamp(targetPos.z, 0, manager.mapSizeY - 1)
            );

        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);
        transform.RotateAround(target.position, Vector3.up, currentYaw);

    }
}
