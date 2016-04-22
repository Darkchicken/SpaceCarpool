using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resource : MonoBehaviour {

    public List<Mesh> resourceMeshes;
    public List<Material> resourceMaterials;

    public void OnChangeToResource(int materialNumber)
    {
        Debug.Log("ITS A RESOURCE");
        
        if (PhotonNetwork.isMasterClient)
        {
            int selector = Random.Range(0, resourceMeshes.Count);
            GetComponent<MeshFilter>().mesh = resourceMeshes[selector];
            int meshNum = selector;
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
            //int matNum = GameManager.gameManager.hitAsteroidMaterialIndex;
            float resourceScale = Random.Range(0.05f, 0.1f);
            transform.localScale = new Vector3(resourceScale, resourceScale, resourceScale);
            GetComponent<MoveObjects>().speed = Random.Range(5, 20);
            GetComponent<PhotonView>().RPC("SetDetails", PhotonTargets.AllBufferedViaServer, meshNum, materialNumber, transform.localScale, GetComponent<MoveObjects>().speed);
        }
    }


    [PunRPC]
    public void Hit(int laserBoltColorIndex)
    {
        GameObject laserParticle = Instantiate(Resources.Load("BlastLaserEffect"), transform.localPosition, transform.rotation) as GameObject;
        laserParticle.GetComponent<ParticleSystemRenderer>().material.color = GameManager.gameManager.laserBoltColors[laserBoltColorIndex];

        DestroyResource();
    }

    [PunRPC]
    public void ReceiveResource()
    {
        //Points will be granted
        PlayFabDataStore.playerScore += 10;
        GameHUDManager.gameHudManager.HudUpdate();
        DestroyResource();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            DestroyResource();
        }
    }

    void DestroyResource()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = GameManager.gameManager.objectPoolingTransform.position;
    }

    [PunRPC]
    public void SetDetails(int meshNumber, int materialNum, Vector3 newScale, float newSpeed)
    {
        tag = "Resource";
        GetComponent<MeshFilter>().mesh = resourceMeshes[meshNumber];
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;

        GetComponent<MeshRenderer>().material = resourceMaterials[materialNum];
        transform.localScale = newScale;
        GetComponent<MoveObjects>().speed = newSpeed;
        GetComponent<MoveObjects>().masterUpdated = true;
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //
    }
}
