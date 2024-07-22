using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public class ImageHandler : NetworkBehaviour
{

    public Material[] images; //this is to change colors at first, will be adjusted to change for the MRI pics
    Renderer rend; //to render the images/colors
    public int imageIndex = 0;
    public int phaseIndex = 0;
    public GameObject cube;
    // Start is called before the first frame update

    public override void OnStartClient()
    {
        base.OnStartClient();
        if(base.IsOwner)
        {
            Debug.Log("You own this shizzle");
        }
        else
        {
            //GetComponent<ImageHandler>().enabled = false;
        }

        if (!base.IsServer)
        {
            GetComponent<ImageHandler>().enabled = false;
        }
    }

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true; // just in case it isn't enabled
        rend.sharedMaterial = images[imageIndex];
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("INDEX DOWN");
            imageIndex -= 1;
            if (imageIndex < 0)
                imageIndex = images.Length - 1;
            changeImage(cube, imageIndex);
            rend.sharedMaterial = images[imageIndex];
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("INDEX UP");
            imageIndex += 1;
            if (imageIndex > images.Length - 1)
                imageIndex = 0;
            changeImage(cube, imageIndex);
            rend.sharedMaterial = images[imageIndex];
        }

        RequestOwnershipOnClick();
    }

    private void RequestOwnershipOnClick()
    {
        if (!base.IsOwner)
            return;

        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            NetworkObject nob = hit.collider.GetComponent<NetworkObject>();

            if(nob != null && !nob.IsOwner)
            {
                Debug.Log("Sending Ownership Rquest for " + hit.collider.gameObject.name);
                ServerRequestOwnership(nob);
            }
        }
    }

    [ServerRpc]
    private void ServerRequestOwnership(NetworkObject nob)
    {
        Debug.Log("Receivd ownership for " + nob.gameObject.name);
        nob.GiveOwnership(base.Owner);
    }

    [ServerRpc]
    public void changeImage(GameObject cube, int imageIndex)
    {
        UpdateImage(cube, imageIndex);
    }

    [ObserversRpc]
    public void UpdateImage(GameObject cube, int imageIndex)
    {
        cube.GetComponent<ImageHandler>().GetComponent<Renderer>().material = images[imageIndex];
    }
}
