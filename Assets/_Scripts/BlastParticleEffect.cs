using UnityEngine;
using System.Collections;

public class BlastParticleEffect : MonoBehaviour {

    private ParticleSystem blastParticle;

    void Awake()
    {
        blastParticle = GetComponent<ParticleSystem>();
        blastParticle.GetComponent<Renderer>().material = GameManager.gameManager.hitObjectMaterial;
        blastParticle.Play();
        StartCoroutine(DestroyParticle());
    }

    IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
