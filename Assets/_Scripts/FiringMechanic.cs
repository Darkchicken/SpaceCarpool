using UnityEngine;
using System.Collections;

public class FiringMechanic : MonoBehaviour {

    private float fireRate = 0.3f;
    private float nextFire;
    private RaycastHit hit;
    private float range;    //set later on, for now we'll use mathf.infinity
    private float raycastLength = 500;
    public LayerMask  layerMask = ~(1 << 8);
    

    public bool animationTrigger = false;


    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        if (Physics.Raycast(ray, out hit, raycastLength, layerMask.value))
        {

            if (hit.collider.tag == "Enemy")
                {

                    Debug.Log("Collided");
                    if (Input.GetMouseButtonDown(0)) //temporary control; switch to tap later
                    {
                        animationTrigger = true;
                        print("I'm looking at " + hit.transform.name);
                        Destroy(hit.collider.gameObject);
                    

                     }
                 }

            else
            {
                print("I'm looking at nothing!");
            }
            Debug.Log("Check");
        }
     
        Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.green);
    }
}

