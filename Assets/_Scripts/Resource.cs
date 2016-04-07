using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resource : MonoBehaviour {

    public List<Mesh> resourceMeshes;
    public List<Material> resourceMaterials;

    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            int selector = Random.Range(0, resourceMeshes.Count);
            GetComponent<MeshFilter>().mesh = resourceMeshes[selector];
            int meshNum = selector;
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
            int matNum = GameManager.gameManager.hitAsteroidMaterialIndex;
            float resourceScale = Random.Range(0.05f, 0.1f);
            transform.localScale = new Vector3(resourceScale, resourceScale, resourceScale);
            GetComponent<MoveObjects>().speed = Random.Range(5, 20);
            GetComponent<PhotonView>().RPC("SetDetails", PhotonTargets.AllBufferedViaServer, meshNum, matNum, transform.localScale, GetComponent<MoveObjects>().speed);
        }
    }


    [PunRPC]
    public void Hit(int laserBoltColorIndex)
    {
        GameObject laserParticle = Instantiate(Resources.Load("BlastLaserEffect"), transform.localPosition, transform.rotation) as GameObject;
        laserParticle.GetComponent<ParticleSystemRenderer>().material.color = GameManager.gameManager.laserBoltColors[laserBoltColorIndex];

        Destroy(gameObject);
    }

    [PunRPC]
    public void ReceiveResource()
    {
        //Points will be granted
        PlayFabDataStore.playerScore += 10;
        GameHUDManager.gameHudManager.SetScore();
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            Destroy(gameObject);
        }
    }

    [PunRPC]
    public void SetDetails(int meshNumber, int materialNum, Vector3 newScale, float newSpeed)
    {
        GetComponent<MeshFilter>().mesh = resourceMeshes[meshNumber];
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;

        GetComponent<MeshRenderer>().material = resourceMaterials[materialNum];
        transform.localScale = newScale;
        GetComponent<MoveObjects>().speed = newSpeed;
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        if (stream.isWriting)
        {
            if (PhotonNetwork.isMasterClient)
            {
                //We own this player: send the others our data
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }

        }
        else
        {
            if (!PhotonNetwork.isMasterClient)
            {
                //Network player, receive data
                objectPos = (Vector3)stream.ReceiveNext();
                objectRot = (Quaternion)stream.ReceiveNext();
            }


        }
        */
    }
}
