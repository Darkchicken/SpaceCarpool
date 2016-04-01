using UnityEngine;
using System.Collections;

public class PlayerCombatManager : MonoBehaviour {

    private RaycastHit hit;
  //  public LayerMask layerMask = ~(1 << 8);    //For use with layermask later on if we need it

    public float fireRate = 0.5f;       //sets the attack speed value
    private float nextFire = 0.0f;

    void Update()
    {
        nextFire += Time.deltaTime;

        //Left Click attack to shoot asteroids
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
            
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.tag == "Asteroid")
                {
                    if (nextFire >= fireRate)   //define "Shoot" button when we get tap to shootand remove mouseButton
                    {
                        nextFire = 0f;
                        
                        //attack speed of weapon
                        //hit.transform.gameObject.GetComponent<Asteroid>().TakeDamage(500);
                        hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.MasterClient, 500);
                    }
                }
                    
            }
        }
    }
}
