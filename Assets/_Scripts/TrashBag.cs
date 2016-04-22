using UnityEngine;
using System.Collections;

public class TrashBag : MonoBehaviour
{

    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            float resourceScale = Random.Range(0.1f, 1f);
            transform.localScale = new Vector3(resourceScale, resourceScale, resourceScale);
            GetComponent<MoveObjects>().speed = Random.Range(5, 40);
            GetComponent<PhotonView>().RPC("SetDetails", PhotonTargets.AllBufferedViaServer, transform.localScale, GetComponent<MoveObjects>().speed);
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
    public void ReceiveTrashBag()
    {
        //Fuel will be granted
        PlayFabDataStore.shipFuel += 2;
        GameHUDManager.gameHudManager.HudUpdate();
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Destination")
        {
            Destroy(gameObject);
        }
    }

    [PunRPC]
    public void SetDetails(Vector3 newScale, float newSpeed)
    {
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
