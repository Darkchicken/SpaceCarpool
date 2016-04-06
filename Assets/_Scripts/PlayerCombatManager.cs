using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombatManager : MonoBehaviour {

    public GameObject laserBolt;
    public Vector3 muzzleOffset;
    
    private Vector3 cameraPosition;
    private RaycastHit hit;
    private Transform muzzleTransform;
  //  public LayerMask layerMask = ~(1 << 8);    //For use with layermask later on if we need it

    public float fireRate = 0.5f;       //sets the attack speed value
    private float nextFire = 0.0f;
    private int counter = 0;

    void Start()
    {
        cameraPosition = Camera.main.transform.position + muzzleOffset;
        muzzleTransform = GameManager.gameManager.muzzleTransform;
    }
    [PunRPC]
    void InstantiateLaserBolt(Vector3 muzzlePos, Quaternion muzzleRot, int laserBoltColorIndex)
    {
        GameObject bolt = GameObject.Instantiate(laserBolt, muzzlePos, muzzleRot) as GameObject;
        bolt.GetComponent<MeshRenderer>().material.color = GameManager.gameManager.laserBoltColors[laserBoltColorIndex];
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
                Debug.Log(PlayFabDataStore.laserBoltColorIndex);
                GetComponent<PhotonView>().RPC("InstantiateLaserBolt", PhotonTargets.All, muzzleTransform.position, muzzleTransform.rotation, PlayFabDataStore.laserBoltColorIndex);
                //Invoke("InstantiateLaserBolt", 0);
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    nextFire = 0f;
                    
                    if (hit.transform.tag == "Asteroid")
                    {
                        GameManager.gameManager.hitObjectMaterial = hit.transform.gameObject.GetComponent<MeshRenderer>().material;
                        Debug.Log("hit count: " + counter++);
                        ApplyDamage();
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
        hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, 1000, PlayFabDataStore.laserBoltColorIndex);
    }
}
