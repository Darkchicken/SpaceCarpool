using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;
    public List<Color> laserBoltColors;
    public BoxCollider spawnArea;
    public BoxCollider destinationArea;
    public Transform muzzleTransform;
    public Material hitObjectMaterial;
    public int hitAsteroidMaterialIndex;
    public GameObject pilotPanel;
    public Text scavengerList;
    GameObject asteroid;
    GameObject resource;
    GameObject returnButton;
    //private Vector3 spawnLocationBoundry;
    private Vector3 colliderSize;
    private Vector3 spawnLocation;

	void Awake ()
    {
        gameManager = this;
        colliderSize = spawnArea.GetComponent<BoxCollider>().size;
        returnButton = GameObject.Find("ReturnToBase");
        

    }

    void Start()
    {
        if(!PhotonNetwork.isMasterClient)
        {
            returnButton.SetActive(false);
        }
        if(PhotonNetwork.isMasterClient)
        {
            pilotPanel.SetActive(true);
            
        }
        StartCoroutine(SpawnObject());
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
           
                string playerList = "Ready to fly:\n";
                foreach (PhotonPlayer player in PhotonNetwork.otherPlayers)
                {
                    playerList += player.name + "\n";
                }
                scavengerList.text = playerList;

        }
    }
    IEnumerator SpawnObject()
    {
        if (PhotonNetwork.isMasterClient)
        {
            int randomNumber = Random.Range(0, 1000);

            if (randomNumber <= 500) //Spawn Asteroid
            {
                spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
                Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
                GameObject asteroidObject = PhotonNetwork.Instantiate("Asteroid", spawnLocation, Quaternion.identity, 0) as GameObject;
            }
            else if (randomNumber <= 0) //Spawn Resource
            {
                spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
                Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
                GameObject resourceObject = PhotonNetwork.Instantiate("Resource", spawnLocation, Quaternion.identity,0) as GameObject;
            }
            else if (randomNumber <= 1000) // Spawn Fuel
            {
            }
        }

        yield return new WaitForSeconds(Random.Range(0, 2));

        StartCoroutine(SpawnObject());

    }
   
    public void ReturnToBase()
    {
        
            Debug.Log("Ending Game");
      
            PhotonNetwork.LoadLevel("Login");
        
       
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ///nothing
    }
}
