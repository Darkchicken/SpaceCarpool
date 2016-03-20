using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;
    public BoxCollider spawnArea;
    public BoxCollider destinationArea;
    public GameObject asteroid;
    public GameObject resource;
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
        int randomNumber = Random.Range(0, 1000);

        if (randomNumber <= 500) //Spawn Asteroid
        {
            spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
            Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
            GameObject asteroidObject = Instantiate(asteroid, spawnLocation, Quaternion.identity) as GameObject;
        }
        else if(randomNumber <= 900) //Spawn Resource
        {
            spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
            Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
            GameObject resourceObject = Instantiate(resource, spawnLocation, Quaternion.identity) as GameObject;
        }
        else if (randomNumber <= 1000) // Spawn Fuel
        {
        }

        yield return new WaitForSeconds(Random.Range(0, 2));

        StartCoroutine(SpawnObject());

    }
}
