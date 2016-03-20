using UnityEngine;
using System.Collections;

public class PlayerCombatManager : MonoBehaviour {

    private RaycastHit hit;

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
                    hit.transform.gameObject.GetComponent<Asteroid>().TakeDamage(500);
                }
                    
            }
        }
    }
}
