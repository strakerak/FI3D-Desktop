using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class ModelController : NetworkBehaviour
{
    public float rotationSpeed = 1f; //rotation speed for movement

    


    void OnMouseDrag()
    {
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.up, XaxisRotation);
        transform.Rotate(Vector3.right, YaxisRotation);

        ChangePos(XaxisRotation, YaxisRotation);
    }

    [ServerRpc(RequireOwnership =false)]
    private void ServerRequestOwnership(NetworkObject nob)
    {
        Debug.Log("Received ownership for " + nob.gameObject.name);
        nob.GiveOwnership(base.Owner);
    }

    [ServerRpc(RequireOwnership =false)]
    public void ChangePos(float X, float Y)
    {
        UpdatePos(X, Y);
    }

    [ObserversRpc]
    public void UpdatePos(float X, float Y)
    {
        transform.Rotate(Vector3.up, X);
        transform.Rotate(Vector3.right, Y);
    }



}
