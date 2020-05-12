using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireMovement : MonoBehaviour
{
    public float speed, rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
    }

    void CheckMovement()
    {
        float move = Input.GetAxis("Vertical");
        float rotate = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(0f, 0f, move * speed);
        Vector3 rotation = new Vector3(0, rotate * rotationSpeed, 0);

        transform.Translate(movement * Time.deltaTime);
        transform.Rotate(rotation * Time.deltaTime);
    }
}
