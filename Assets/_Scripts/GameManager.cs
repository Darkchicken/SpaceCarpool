using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Transform spawnLocation;
    public GameObject moon;
    private Vector3 spawnLocationBoundry;

	void Start ()
    {
        spawnLocationBoundry = spawnLocation.gameObject.GetComponent<Collider>().bounds.size;
        Invoke("SpawnObject", 5f);
        Invoke("SpawnObject", 10f);
        Invoke("SpawnObject", 15f);
        Invoke("SpawnObject", 20f);
    }

    void SpawnObject()
    {
        Instantiate(moon, spawnLocation.position, Quaternion.identity);
    }
}
