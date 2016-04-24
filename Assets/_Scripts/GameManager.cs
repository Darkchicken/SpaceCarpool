using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;
    public int pooledAmount = 20;
    public Transform objectPoolingTransform;
    public List<GameObject> poolObjectList;
    public List<Color> laserBoltColors;
    public BoxCollider spawnArea;
    public BoxCollider destinationArea;
    public Transform muzzleTransform;
    public Material hitObjectMaterial;
    public PhotonView photonView;
    public int hitAsteroidMaterialIndex;

    
    public bool isGameStarted = false;

    GameObject asteroid;
    GameObject resource;
    GameObject returnButton;
    //private Vector3 spawnLocationBoundry;
    private Vector3 colliderSize;
    private Vector3 spawnLocation;
    

	void Awake ()
    {
        gameManager = this;
        photonView = GetComponent<PhotonView>();
        colliderSize = spawnArea.GetComponent<BoxCollider>().size;
        returnButton = GameObject.Find("ReturnToBase");
        poolObjectList = new List<GameObject>();
        

    }

    void Start()
    {
        if(!PhotonNetwork.isMasterClient)
        {
            //returnButton.SetActive(false);
        }
        if(PhotonNetwork.isMasterClient)
        {
            InitializeThePlayer();
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject asteroidObject = PhotonNetwork.Instantiate("Asteroid", objectPoolingTransform.position, Quaternion.identity, 0) as GameObject;
                
            }

            StartCoroutine(SpawnObject());

        }

    }

    public void InitializeThePlayer()
    {
        string spawnpointName = "SpawnPoint" + (PlayFabDataStore.laserBoltColorIndex + 1);
        GameObject spawnPoint = GameObject.Find(spawnpointName);
        //instantiate player on all clients
        Debug.Log(spawnpointName);
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.transform.position, Quaternion.identity, 0);
       

        Debug.Log("Initial Values Set!");
        PlayFabDataStore.shipHealth = PlayFabDataStore.shipHealthMax;
        PlayFabDataStore.shipFuel = PlayFabDataStore.shipFuelMax;
        GameHUDManager.gameHudManager.HudUpdate();
    }


    IEnumerator SpawnObject()
    {

        if(isGameStarted)
        {
            int randomNumber = Random.Range(0, 1000);

            if (randomNumber <= 950) //Spawn Asteroid
            {
                for (int i = 0; i < poolObjectList.Count; i++)
                {
                    if (!poolObjectList[i].activeInHierarchy)
                    {
                        spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
                        Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
                        poolObjectList[i].transform.position = spawnLocation;
                        poolObjectList[i].SetActive(true);
                        break;
                    }
                }
            }
            else if (randomNumber <= 1000) //Spawn TrashBag
            {
                spawnLocation = new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
                Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2), spawnArea.transform.position.z);
                GameObject resourceObject = PhotonNetwork.Instantiate("TrashBag", spawnLocation, Quaternion.identity, 0) as GameObject;
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

    [PunRPC]
    void EnableObject(int viewID, Vector3 position)
    {
        for (int i = 0; i < poolObjectList.Count; i++)
        {
            if (!poolObjectList[i].activeInHierarchy)
            {
                poolObjectList[i].SetActive(true);
                poolObjectList[i].transform.position = position;
                Debug.Log("EnableObject");
                break;
            }
        }
    }
}
