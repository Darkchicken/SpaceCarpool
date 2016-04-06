using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;
    public BoxCollider spawnArea;
    public BoxCollider destinationArea;
    public Transform muzzleTransform;
    public Material hitObjectMaterial;
    GameObject asteroid;
    GameObject resource;
    //private Vector3 spawnLocationBoundry;
    private Vector3 colliderSize;
    private Vector3 spawnLocation;

	void Awake ()
    {
        gameManager = this;
        colliderSize = spawnArea.GetComponent<BoxCollider>().size;
    }

    void Start()
    {
        StartCoroutine(SpawnObject());
    }


    IEnumerator SpawnObject()
    {
        if (PhotonNetwork.isMasterClient)
        {
            int randomNumber = Random.Range(0, 1000);

            if (randomNumber <= 500) //Spawn Asteroid
            {
                spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
                Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
                GameObject asteroidObject = PhotonNetwork.Instantiate("Asteroid", spawnLocation, Quaternion.identity, 0) as GameObject;
            }
            else if (randomNumber <= 900) //Spawn Resource
            {
                spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
                Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
                GameObject resourceObject = PhotonNetwork.Instantiate("Resource", spawnLocation, Quaternion.identity,0) as GameObject;
            }
            else if (randomNumber <= 1000) // Spawn Fuel
            {
            }
        }

        yield return new WaitForSeconds(Random.Range(0, 2));

        StartCoroutine(SpawnObject());

    }
}
