using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public BoxCollider spawnArea;
    public GameObject moon;
    //private Vector3 spawnLocationBoundry;
    private Vector3 colliderSize;
    private Vector3 spawnLocation;

	void Start ()
    {
        //spawnLocationBoundry = spawnLocation.gameObject.GetComponent<Collider>().bounds.size;
        colliderSize = spawnArea.GetComponent<BoxCollider>().size;

    }

    void Update()
    {
        Invoke("SpawnObject", 3f);
    }

    void SpawnObject()
    {
        
        spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
            Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
        Instantiate(moon, spawnLocation, Quaternion.identity);
    }
}
