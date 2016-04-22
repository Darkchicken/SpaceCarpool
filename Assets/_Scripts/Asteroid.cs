using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour
{
    public int health;
    public List<Mesh> asteroidMeshes;
    public List<Material> asteroidMaterials;
    public Transform objectPoolingTransform;

    private int materialIndex;
    private PhotonView photonView;

    private static int counter = 0;
    private int damage;


    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        GameManager.gameManager.poolObjectList.Add(gameObject);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        tag = "Asteroid";
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Asteroid Enabled!");
            GameManager.gameManager.photonView.RPC("EnableObject", PhotonTargets.Others, photonView.viewID, transform.position);

            int selector = Random.Range(0, asteroidMeshes.Count);
            GetComponent<MeshFilter>().mesh = asteroidMeshes[selector];
            int meshNum = selector;
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
            selector = Random.Range(0, asteroidMaterials.Count);
            GetComponent<MeshRenderer>().material = asteroidMaterials[selector];
            int matNum = selector;
            materialIndex = selector;
            float asteroidScale = Random.Range(0.1f, 2);
            transform.localScale = new Vector3(asteroidScale, asteroidScale, asteroidScale);
            GetComponent<MoveObjects>().speed = Random.Range(25, 100);
            health = (int)(100 * asteroidScale * 3);
            photonView.RPC("SetDetails",PhotonTargets.AllBufferedViaServer, meshNum,matNum, transform.position, transform.localScale, GetComponent<MoveObjects>().speed, health);
            photonView.RPC("SetDirection", PhotonTargets.AllViaServer, GetComponent<MoveObjects>().endPosition);
        }

        
    }

    [PunRPC]
    void EnableAsteroid()
    {
        //.SetActive(true);
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destination"))
        {
            DestroyAsteroid();
        }
        else if(other.CompareTag("Ship"))
        {
            other.GetComponent<ShipControl>().TakeDamage(damage);
        }
    }

    [PunRPC]
    public void TakeDamage(int getDamage, int laserBoltColorIndex)
    {
        GameObject laserParticle = Instantiate(Resources.Load("BlastLaserEffect"), transform.localPosition, transform.rotation) as GameObject;
        laserParticle.GetComponent<ParticleSystemRenderer>().material.color = GameManager.gameManager.laserBoltColors[laserBoltColorIndex];

        if (health - getDamage > 0)
        {
            health -= getDamage;
        }
        else
        {

            GameObject asteroidParticle = Instantiate(Resources.Load("BlastAsteroidEffect"), transform.localPosition, transform.rotation) as GameObject;
            asteroidParticle.GetComponent<ParticleSystemRenderer>().material = GetComponent<MeshRenderer>().material;
            
            if(PhotonNetwork.isMasterClient)
            {
                //GameObject resource = PhotonNetwork.Instantiate("Resource", transform.position, transform.rotation, 0) as GameObject;
                GetComponent<Resource>().OnChangeToResource(materialIndex);
                GetComponent<MoveObjects>().ResourceLoad();
                GameManager.gameManager.hitAsteroidMaterialIndex = materialIndex;
            }

            //DestroyAsteroid();
        }
    }

    void DestroyAsteroid()
    {
        Debug.Log("ASteroid Destroyed");
        gameObject.SetActive(false);
        gameObject.transform.position = GameManager.gameManager.objectPoolingTransform.position;
    }


    [PunRPC]
    public void SetDetails(int meshNumber,int materialNum, Vector3 position, Vector3 newScale, float newSpeed, int newHealth)
    {
        Debug.Log("SETDETAILS");
        transform.position = position;
        GetComponent<MeshFilter>().mesh = asteroidMeshes[meshNumber];
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
      
        GetComponent<MeshRenderer>().material = asteroidMaterials[materialNum];
        transform.localScale = newScale;
        GetComponent<MoveObjects>().speed = newSpeed;
        health = newHealth;
        damage = Mathf.CeilToInt(newScale.x) * 10;
        GetComponent<MoveObjects>().masterUpdated = true;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //
    }

}
