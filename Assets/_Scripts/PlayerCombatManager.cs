using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombatManager : MonoBehaviour {

    public AudioSource laserSound;
    public AudioSource tractorSound;
    public AudioSource resourceSound;

    public Vector3 muzzleOffset;
    public float fireRate = 0.5f;
    public float beamCatchTime = 2;

    private Vector3 cameraPosition;
    private RaycastHit hit;
    private Transform muzzleTransform;

    private float nextFire = 0;
    private int counter = 0;

    private bool isFireWeapon = true;
    private float beamTimer = 0;
    private float beamLength = 0;
    private bool isBeamBroken = false;

    //private GameObject tempBeam;

    void Start()
    {
        cameraPosition = Camera.main.transform.position + muzzleOffset;
        muzzleTransform = GameManager.gameManager.muzzleTransform;
        
    }

    

    [PunRPC]
    void InstantiateLaserBolt(Vector3 muzzlePos, Quaternion muzzleRot, int laserBoltColorIndex)
    {
        GameObject bolt = GameObject.Instantiate(Resources.Load("BasicBeamShot"), muzzlePos, muzzleRot) as GameObject;
        bolt.GetComponent<BeamParam>().BeamColor = GameManager.gameManager.laserBoltColors[laserBoltColorIndex];
        //bolt.GetComponent<MeshRenderer>().material.color = GameManager.gameManager.laserBoltColors[laserBoltColorIndex];
    }
    [PunRPC]
    void InstantiateTractorBeam(Vector3 muzzlePos, Quaternion muzzleRot, int laserBoltColorIndex, float distance)
    {
        GameObject beam = Instantiate(Resources.Load("GeroBeam"), muzzlePos, muzzleRot) as GameObject;
        beam.GetComponent<BeamParam>().BeamColor = GameManager.gameManager.laserBoltColors[laserBoltColorIndex];
        //tempBeam = beam;
        beam.transform.parent = muzzleTransform;
        beam.GetComponent<BeamParam>().MaxLength = distance;
        //beam.GetComponent<MeshRenderer>().material.color = GameManager.gameManager.laserBoltColors[laserBoltColorIndex];
        //beam.GetComponent<VolumetricLineBehavior>().EndPos = new Vector3(0, 0, distance);
    }
    void Update()
    {
        nextFire += Time.deltaTime;

        //Left Click attack to shoot asteroids
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (isFireWeapon)
            {
                if (nextFire >= fireRate)   //define "Shoot" button when we get tap to shootand remove mouseButton
                {
                    Debug.Log(PlayFabDataStore.laserBoltColorIndex);
                    GetComponent<PhotonView>().RPC("InstantiateLaserBolt", PhotonTargets.All, muzzleTransform.position, muzzleTransform.rotation, PlayFabDataStore.laserBoltColorIndex);
                    laserSound.Play();
                    if (Physics.Raycast(ray, out hit, 1000))
                    {
                        nextFire = 0;

                        if (hit.transform.tag == "Asteroid")
                        {
                            GameManager.gameManager.hitObjectMaterial = hit.transform.gameObject.GetComponent<MeshRenderer>().material;
                            Debug.Log("hit count: " + counter++);
                            ApplyDamage();
                        }
                        else if (hit.transform.CompareTag("Resource") || hit.transform.CompareTag("TrashBag"))
                        {
                            HitObject();
                        }
                    }

                }
            }
            else
            {
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.transform.CompareTag("Resource") || hit.transform.CompareTag("TrashBag"))
                    {
                        beamLength = Vector3.Distance(muzzleTransform.position, hit.transform.position);
                        GetComponent<PhotonView>().RPC("InstantiateTractorBeam", PhotonTargets.All, muzzleTransform.position, muzzleTransform.rotation, PlayFabDataStore.laserBoltColorIndex, beamLength);
                        tractorSound.Play();
                    }
                        
                }   
                beamTimer = 0;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!isFireWeapon)
            {
                beamTimer += Time.deltaTime;
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.transform.tag == "Resource")
                    {
                        if(beamTimer >= beamCatchTime )
                        {
                            CatchResource();
                        }
                    }
                    else
                    if (hit.transform.tag == "TrashBag")
                    {
                        if (beamTimer >= beamCatchTime)
                        {
                            CatchTrashBag();
                        }
                    }
                    else
                    {
                        beamTimer = 0;
                    }
                }

           }
           
        }
        else
        {
            tractorSound.Stop();
        }
    }

    public void SetWeapon(bool select)
    {
        isFireWeapon = select;
    }

    void ApplyDamage()
    {
        hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, 1000, PlayFabDataStore.laserBoltColorIndex);
    }

    void HitObject()
    {
        hit.transform.gameObject.GetComponent<PhotonView>().RPC("Hit", PhotonTargets.All, PlayFabDataStore.laserBoltColorIndex);
    }

    void CatchResource()
    {
        resourceSound.Play();
        hit.transform.gameObject.GetComponent<PhotonView>().RPC("ReceiveResource", PhotonTargets.All);
    }

    void CatchTrashBag()
    {
        resourceSound.Play();
        hit.transform.gameObject.GetComponent<PhotonView>().RPC("ReceiveTrashBag", PhotonTargets.All);
    }
}
