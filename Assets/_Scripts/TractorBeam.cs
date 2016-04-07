using UnityEngine;
using System.Collections;

public class TractorBeam : MonoBehaviour {

    void Start()
    {
        StartCoroutine(DestroyBeam());
    }


    IEnumerator DestroyBeam()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
