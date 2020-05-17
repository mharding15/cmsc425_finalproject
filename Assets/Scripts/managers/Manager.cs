using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Manager : MonoBehaviour
{
    //used for global variables and persistant behavior

    public static Manager Instance { get; private set; }

    

    public float camSpeed = 10.0f;
    public float rotationSpeed = 100.0f;
    public GameObject center;

    //public inputMap controls;


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

    public void Update()
    {
        //controls.handleInputs();
    }



}


