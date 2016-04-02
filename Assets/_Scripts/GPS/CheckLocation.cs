using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckLocation : MonoBehaviour {

    static string masterPlayfabId;
    GameObject debugObject;
    public Text debugText;
    bool updatePosition = false;
    //distance a player can be from host before being out of range
    float variance = 0.001f;
    //timer to check how long player has been out of range
    float distanceTimer = 0;
    //player will be kicked if out of range for 30 sec
    float timeToKick = 30f;
    //checks if player is currently in range of host
    bool inRange;
    //last timestamp
    double lastUpdate = 0;
    PhotonView photonView;

    public float myLon;
    public float myLat;
    public float masterLon;
    public float masterLat;
    double currentSpeed = 0;
    bool updating = false;

    void Awake()
    {
        if(GameObject.Find("DebugText") != null)
        {
            debugObject = GameObject.Find("DebugText");
            debugText = GameObject.Find("DebugText").GetComponent<Text>();
        }
    }
    IEnumerator Start()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }

        // Start service before querying location
        Input.location.Start(10,0);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {

            // Access granted and location value could be retrieved
            updatePosition = true;

        }
        photonView = GetComponent<PhotonView>();
        //set initial value for last update
        lastUpdate = Input.location.lastData.timestamp;
        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }

    void Update()
    {
        //toggles debug text on and off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //activate debug text
            debugObject.SetActive(!debugObject.activeSelf);

        }
        if (updatePosition == true)
        {
            if(updating == false)
            {
                InvokeRepeating("UpdateMasterPos", 1, 3);
                updating = true;
            }
            //debugText.text = ("Location (lat/long/alt): " + Input.location.lastData.latitude + " / " + Input.location.lastData.longitude + " / " + Input.location.lastData.altitude
             //  + " ----Horizontal accurancy/ timestamp: " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            
            if (PhotonNetwork.isMasterClient)
            {
                //check speed of host to be sure that host is in a car
                if(lastUpdate != Input.location.lastData.timestamp)
                {
                    currentSpeed = CheckSpeed();
                }
                
                //set position of host
                masterLon = myLon = Input.location.lastData.longitude;
                masterLat = myLat = Input.location.lastData.latitude;
                
              
                //Host is always in range of themself
                inRange = true;
            }
            //if this user in not the host
            else
            {
                //update your position
                myLon = Input.location.lastData.longitude;
                myLat = Input.location.lastData.latitude;
                //if the player is in a room with a host
                if (PhotonNetwork.inRoom)
                {
                    //check range, increment timer if not in range
                    TimeInRange();
                   
                    //check distance to host
                    CompareLocation();
                }


            }
            ///FOR TESTING PURPOSES ONLY////////////////////////////////////
            if (SceneManager.GetActiveScene().name == "TestGPS" || SceneManager.GetActiveScene().name == "TestSpawning")
            {

                debugText.text = ("Location (lat/long): " + myLat + " / " + myLon
                  + "\n timestamp: " + Input.location.lastData.timestamp
                  + "\n time since last update " + (Input.location.lastData.timestamp - lastUpdate)
                  + "\n Master (lat/long): " + masterLat + " / " + masterLon
                  + "\n In range? -> " + inRange
                  + "\n Time out of range: " + distanceTimer
                  + "\n distance = " + GetDistance()
                  + "\n speed = " + currentSpeed
                   + "\n Master? = " + PhotonNetwork.isMasterClient);

            }
            ////////////////////////////////////////////////////////////////       
        }
    }

    void OnApplicationQuit()
    {
        Input.location.Stop();
    }
    public float GetDistance()
    {
        //calculates distance between host and guest with longitude and latitude using distance formula
        return Mathf.Sqrt(Mathf.Pow((masterLon-myLon),2)+Mathf.Pow((masterLat-myLat),2));
    }
    //checks to see if host is in a car by comparing location over a period of time
    public double CheckSpeed()
    {
        if(!PhotonNetwork.isMasterClient)
        { return 0; }
        //check distance travelled based on longitude and latitude (and altitude?) over a set period of time
        //return true if in a car, return false if not
        double distance = Mathf.Sqrt(Mathf.Pow((myLon- Input.location.lastData.longitude), 2) + Mathf.Pow((myLat- Input.location.lastData.latitude), 2));
        double time = Input.location.lastData.timestamp-lastUpdate;
        double speed = (distance / time);
        lastUpdate = Input.location.lastData.timestamp;
        return speed;

    }

    public void TimeInRange()
    {
        //if player is out of range of host, increment timer
        if (inRange == false)
        {
            distanceTimer += Time.deltaTime;
            //if the timer is greater than the max time out of range
            if (distanceTimer > timeToKick)
            {
                debugText.text = ("You have been removed from room for leaving car");
                Debug.Log("You have been removed from room for leaving car");
                PhotonNetwork.LeaveRoom();
            }
        }
        //if they are in range, reset timer
        else
        {
            distanceTimer = 0;
        }
       
       
    }
    public void UpdateMasterPos()
    {
        photonView.RPC("UpdateMaster", PhotonTargets.All, myLon, myLat);
    }
    public void CompareLocation()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("No need to compare master position to itself");
            inRange = true;
        }
        else
        {
           
            if(GetDistance() > variance)
            {
                inRange = false;
            }
            else
            {
                inRange = true;
            }
        }  
    }
    public bool CompareLocationOnJoin()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("No need to compare master position to itself");
            return true;
        }
        if (GetDistance() > variance)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    /*
    public void StoreMasterData()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Dictionary<string, string> customData = new Dictionary<string, string>(); //create a dictionary for the data
            string locationString = masterLat + "#" + masterLon;
            customData.Add("Location", locationString); //locationString has to be this way: Latitude#Longtitude
            PlayFabApiCalls.UpdateUserLocation(customData); // then call this function and send the dictionary
        }
    }
    //this puts the value from playfab into the variables in game
    public void UpdateMasterData()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            if (masterPlayfabId != null)
            {
                PlayFabApiCalls.GetUserLocation(masterPlayfabId); // then call this function and send the dictionary
            }
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Debug.Log("Is Writing");
            if (PhotonNetwork.isMasterClient)
            {
                stream.SendNext(masterLon);
                stream.SendNext(masterLat);
            }
        }
        else
        {
            Debug.Log("Is Reading");
            masterLon= (float)stream.ReceiveNext();
            masterLat = (float)stream.ReceiveNext();
        }
    }
    */
    [PunRPC]
    public void UpdateMaster(float lon, float lat)
    {
        if (photonView.isMine)
        {
            masterLon = lon;
            masterLat = lat;
            debugText.text = ("Received Update from Master"
                + "\n MasterLon = " + masterLon
                + "\n MasterLat = " + masterLat);
        }
    }

}
