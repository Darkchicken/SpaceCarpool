using UnityEngine;
using System.Collections;

public class LaserBolt : MonoBehaviour {

    private static int counter = 0;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(DestroyBolt());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 2000f;

    }

    IEnumerator DestroyBolt()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
