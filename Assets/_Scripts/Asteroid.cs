using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour
{
    public int health;
    Vector3 objectPos;
    Quaternion objectRot;
    public List<Mesh> asteroidMeshes;
    public List<Material> asteroidMaterials;

    private static int counter = 0;


    void Start()
    {
        objectPos = transform.position;
        objectRot = Quaternion.identity;
        if (PhotonNetwork.isMasterClient)
        {
            int selector = Random.Range(0, asteroidMeshes.Count);
            GetComponent<MeshFilter>().mesh = asteroidMeshes[selector];
            int meshNum = selector;
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
            selector = Random.Range(0, asteroidMaterials.Count);
            GetComponent<MeshRenderer>().material = asteroidMaterials[selector];
            int matNum = selector;
            float asteroidScale = Random.Range(0.1f, 2);
            transform.localScale = new Vector3(asteroidScale, asteroidScale, asteroidScale);
            GetComponent<MoveObjects>().speed = Random.Range(50, 150);
            health = (int)(100 * asteroidScale * 3);
            GetComponent<PhotonView>().RPC("SetDetails",PhotonTargets.AllBufferedViaServer, meshNum,matNum, transform.localScale, GetComponent<MoveObjects>().speed, health);
        }

        
    }
    void Update()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            transform.position = Vector3.Lerp(transform.position, objectPos, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, objectRot, 0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Destination" && PhotonNetwork.isMasterClient)
        {

            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void TakeDamage(int getDamage, int laserBoltColorIndex)
    {
        GameObject LaserParticle = GameObject.Instantiate(Resources.Load("BlastLaserEffect"), transform.localPosition, transform.rotation) as GameObject;
        LaserParticle.GetComponent<ParticleSystemRenderer>().material.color = GameManager.gameManager.laserBoltColors[laserBoltColorIndex];

        if (health - getDamage > 0)
        {
            health -= getDamage;
        }
        else
        {

            //GetComponent<PhotonView>().RPC("DestroyAsteroid", PhotonTargets.All);
            GameObject asteroidParticle = GameObject.Instantiate(Resources.Load("BlastAsteroidEffect"), transform.localPosition, transform.rotation) as GameObject;
            asteroidParticle.GetComponent<ParticleSystemRenderer>().material = GetComponent<MeshRenderer>().material;
            Destroy(gameObject);
        }




        /*if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Asteroid Took Damage!");
            if (health - getDamage > 0)
            {
                health -= getDamage;
            }
            else
            {

                GetComponent<PhotonView>().RPC("DestroyAsteroid", PhotonTargets.All);
                PhotonNetwork.Destroy(gameObject);
            }
        }*/
    }


    [PunRPC]
    public void SetDetails(int meshNumber,int materialNum, Vector3 newScale, float newSpeed, int newHealth)
    {
        GetComponent<MeshFilter>().mesh = asteroidMeshes[meshNumber];
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
      
        GetComponent<MeshRenderer>().material = asteroidMaterials[materialNum];
        transform.localScale = newScale;
        GetComponent<MoveObjects>().speed = newSpeed;
        health = newHealth;
    }
   /* 
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
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
    }
    */
    

}
