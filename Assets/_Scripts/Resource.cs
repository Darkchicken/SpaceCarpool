using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {

    public Mesh[] resourceMeshes;
    public Material[] resourceMaterials;

    void Start()
    {
        /*int selector = Random.Range(0, resourceMeshes.Length);
        GetComponent<MeshFilter>().mesh = resourceMeshes[selector];
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        selector = Random.Range(0, resourceMaterials.Length);
        GetComponent<MeshRenderer>().material = resourceMaterials[selector];*/
        float asteroidScale = Random.Range(0.01f, 0.1f);
        transform.localScale = new Vector3(asteroidScale, asteroidScale, asteroidScale);
        GetComponent<MoveObjects>().speed = Random.Range(100, 300);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            Destroy(gameObject);
        }
        if(other.tag == "LaserBolt")
        {
            Destroy(gameObject);
        }
    }
}
