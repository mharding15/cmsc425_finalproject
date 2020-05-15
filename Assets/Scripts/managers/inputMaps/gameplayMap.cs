using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameplayMap : inputMap
{
    public override void handleInputs()
    {

        float vMag = Input.GetAxis("Vertical");
        float hMag = Input.GetAxis("Horizontal");

        //var mainCam = GameObject.FindWithTag("MainCamera");
        //Vector3 mvmt = new Vector3(hMag, vMag, 0.0f);
        //float camSpeed = Manager.Instance.camSpeed;

        //mainCam.transform.position = mainCam.transform.position + (mvmt * camSpeed * Time.deltaTime);

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Left Click Registered by inputMap");
        }

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Right Click Registered by inputMap");
        }

    }
}
