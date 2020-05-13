using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    //used for global variables and persistant behavior
    
    public static Manager Instance { get; private set; }

    public int mapSizeX = 10;
    public int mapSizeY = 10;



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
}
