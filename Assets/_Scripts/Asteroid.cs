using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    public int health;
    public Mesh[] asteroidMeshes;
    public Material[] asteroidMaterials;

    void Start()
    {
        int selector = Random.Range(0, asteroidMeshes.Length);
        GetComponent<MeshFilter>().mesh = asteroidMeshes[selector];
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        selector = Random.Range(0, asteroidMaterials.Length);
        GetComponent<MeshRenderer>().material = asteroidMaterials[selector];
        float asteroidScale = Random.Range(0.1f, 2);
        transform.localScale = new Vector3(asteroidScale, asteroidScale, asteroidScale);
        GetComponent<MoveObjects>().speed = Random.Range(100, 300);
        health = (int)(100 * asteroidScale * 3);
        
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Asteroid Took Damage!");
        if(health - damage > 0)
        {
            health -= damage;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Destination")
        {
            Destroy(gameObject);
        }
    }

}
