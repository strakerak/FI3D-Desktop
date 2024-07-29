using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;
using System.IO;

public class SliceHandler : NetworkBehaviour
{

    public Material[] images; //this is to change colors at first, will be adjusted to change for the MRI pics
    Renderer rend; //to render the images/colors

    public GameObject[] endos;
    public GameObject[] epis;

    public int imageIndex = 0;
    public int phaseIndex = 0;
    public GameObject cube;

    public GameObject modelObject;
    public GameObject endoObject;
    public GameObject epiObject;

    GameObject epiTemp;
    GameObject endoTemp;

    // Start is called before the first frame update

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            Debug.Log("You own this slice");
        }
        else
        {
            Debug.Log("NO OWNERSHIP OMG! FOR THE SLICE");
        }

    }

    private void Start()
    {
        //rend = GetComponent<Renderer>();
        //rend.enabled = true; // just in case it isn't enabled
        //rend.sharedMaterial = images[imageIndex];


        epiObject = Instantiate(epis[0], cube.transform.position, cube.transform.rotation, modelObject.transform);
        endoObject = Instantiate(endos[0], cube.transform.position, cube.transform.rotation, modelObject.transform);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("INDEX DOWN");
            imageIndex -= 1;
            if (imageIndex < 0)
                imageIndex = endos.Length - 1;
            //changeImage(cube, imageIndex);
            //rend.sharedMaterial = images[imageIndex];
            epiTemp = epiObject;
            endoTemp = endoObject;

            epiObject = Instantiate(epis[imageIndex], cube.transform.position, cube.transform.rotation, modelObject.transform);
            endoObject = Instantiate(endos[imageIndex], cube.transform.position, cube.transform.rotation, modelObject.transform);
            
            Destroy(epiTemp);
            Destroy(endoTemp);
            changePhase(cube, imageIndex);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("INDEX UP");
            imageIndex += 1;
            if (imageIndex > endos.Length - 1)
                imageIndex = 0;
            //changeImage(cube, imageIndex);
            //rend.sharedMaterial = images[imageIndex];
            epiTemp = epiObject;
            endoTemp = endoObject;

            epiObject = Instantiate(epis[imageIndex], cube.transform.position, cube.transform.rotation, modelObject.transform);
            endoObject = Instantiate(endos[imageIndex], cube.transform.position, cube.transform.rotation, modelObject.transform);
            
            Destroy(epiTemp);
            Destroy(endoTemp);
            changePhase(cube, imageIndex);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            RequestOwnershipOnClick();
        }
    }

    private void RequestOwnershipOnClick()
    {

        /*if (!base.IsOwner)
        {
            Debug.Log("WE OUT");
            return;
        }*/


        NetworkObject nob = cube.GetComponent<NetworkObject>();

        if (nob != null && !nob.IsOwner)
        {
            Debug.Log("Sending Ownership Request for " + cube.gameObject.name);
            ServerRequestOwnership(nob);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerRequestOwnership(NetworkObject nob)
    {
        Debug.Log("Received ownership for " + nob.gameObject.name);
        nob.GiveOwnership(base.Owner);
    }

    [ServerRpc(RequireOwnership = false)]
    public void changeImage(GameObject cube, int imageIndex)
    {
        UpdateImage(cube, imageIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    private void changePhase(GameObject cube, int phase)
    {
        UpdatePhase(cube, phase);
    }

    [ObserversRpc]
    public void UpdateImage(GameObject cube, int imageIndexD)
    {
        cube.GetComponent<SliceHandler>().GetComponent<Renderer>().material = images[imageIndexD];
        imageIndex = imageIndexD;
    }

    [ObserversRpc]
    public void UpdatePhase(GameObject cube, int phase)
    {
        epiTemp = epiObject;
        endoTemp = endoObject;

        epiObject = Instantiate(epis[imageIndex], cube.transform.position, cube.transform.rotation, modelObject.transform);
        endoObject = Instantiate(endos[imageIndex], cube.transform.position, cube.transform.rotation, modelObject.transform);
        
        Destroy(epiTemp);
        Destroy(endoTemp);
        imageIndex = phase;
    }
}
