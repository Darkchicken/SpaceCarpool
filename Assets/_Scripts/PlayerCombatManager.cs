using UnityEngine;
using System.Collections;

public class PlayerCombatManager : MonoBehaviour {

    private RaycastHit hit;
  //  public LayerMask layerMask = ~(1 << 8);    //For use with layermask later on if we need it

    public float fireRate = 0.5F;       //sets the attack speed value
    private float nextFire = 0.0F;

    void Update()
    {
        //Left Click attack to shoot asteroids
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
            
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.tag == "Asteroid")
                {
                    if (Input.GetMouseButtonDown(0)/*Input.GetButton("Shoot")*/ && Time.time > nextFire)   //define "Shoot" button when we get tap to shootand remove mouseButton
                    {
                        nextFire = Time.time + fireRate;                    //attack speed of weapon
                        //hit.transform.gameObject.GetComponent<Asteroid>().TakeDamage(500);
                        hit.transform.gameObject.GetComponent<PhotonView>().RPC("gethit", PhotonTargets.MasterClient, 500);
                    }
                }
                    
            }
        }
    }
}
