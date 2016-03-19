using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;
    public BoxCollider spawnArea;
    public BoxCollider destinationArea;
    public GameObject moon;
    //private Vector3 spawnLocationBoundry;
    private Vector3 colliderSize;
    private Vector3 spawnLocation;

	void Awake ()
    {
        gameManager = this;
        //spawnLocationBoundry = spawnLocation.gameObject.GetComponent<Collider>().bounds.size;
        colliderSize = spawnArea.GetComponent<BoxCollider>().size;

    }

    void Update()
    {
        SpawnObject();
    }

    void SpawnObject()
    {
        
        spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
            Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
        GameObject obj = Instantiate(moon, spawnLocation, Quaternion.identity) as GameObject;
        obj.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
