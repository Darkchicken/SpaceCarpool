using UnityEngine;
using System.Collections;

public class LaserBolt : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DestroyBeam());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 2000f;

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Asteroid"))
        {
            other.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.MasterClient, 500);
            Destroy(gameObject);
        }
        else if(other.CompareTag("Resource"))
        {
            Destroy(gameObject);
        }
    }
    IEnumerator DestroyBeam()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
