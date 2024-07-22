using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class ModelController : NetworkBehaviour
{
    float rotationSpeed = 1f; //rotation speed for movement

    


    void OnMouseDrag()
    {
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.up, XaxisRotation);
        transform.Rotate(Vector3.right, YaxisRotation);
    }

    

}
