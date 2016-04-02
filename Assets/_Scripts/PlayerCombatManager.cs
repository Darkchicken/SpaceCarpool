using UnityEngine;
using System.Collections;

public class PlayerCombatManager : MonoBehaviour {

    public GameObject laserBolt;
    public Vector3 muzzleOffset;
    private Vector3 cameraPosition;
    private RaycastHit hit;
    private Transform muzzleTransform;
  //  public LayerMask layerMask = ~(1 << 8);    //For use with layermask later on if we need it

    public float fireRate = 0.5f;       //sets the attack speed value
    private float nextFire = 0.0f;

    void Start()
    {
        cameraPosition = Camera.main.transform.position + muzzleOffset;
        muzzleTransform = GameManager.gameManager.muzzleTransform;
    }

    void InstantiateLaserBolt()
    {
        GameObject bolt = GameObject.Instantiate(laserBolt, muzzleTransform.position, muzzleTransform.rotation) as GameObject;
    }
    void Update()
    {
        nextFire += Time.deltaTime;

        //Left Click attack to shoot asteroids
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);

            if (nextFire >= fireRate)   //define "Shoot" button when we get tap to shootand remove mouseButton
            {
                Invoke("InstantiateLaserBolt", 0);
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    nextFire = 0f;
                    if (hit.transform.tag == "Asteroid")
                    {
                        Debug.Log("Raycast Hit Asteroid");
                        Invoke("ApplyDamage", 0.1f);
                    }
                    if(hit.transform.tag == "Resource")
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
                    
            }
        }
    }

    void ApplyDamage()
    {
        hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.MasterClient, 500);
    }
}
