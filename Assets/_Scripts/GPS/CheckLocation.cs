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
    //distance a player can be from host before being out of range (in feet)
    float variance = 10f;
    //timer to check how long player has been out of range
    float distanceTimer = 0;
    //timer to check how long player has been below speed limit
    float speedTimer = 0;
    //player will be kicked if out of range for 30 sec
    float timeToKick = 30f;
    //points will cease being added if under speed limit for 60 sec
    float timeToEndPoints = 60f;
    //checks if player is currently in range of host
    bool inRange;
    //last timestamp
    double lastUpdate = 0;
    double currentTime = 0;

    PhotonView photonView;

    public double lastLon = 0;
    public double lastLat = 0;
    public double myLon = 0 ;
    public double myLat = 0;
    public double masterLon = 0;
    public double masterLat = 0;
    double currentSpeed = 0;
    bool updating = false;
    //check if players should be earning points
    //sets to true upon reaching speed limit
    bool earningPoints = false;
    //multiply the current speed by this since the speed variable is E-05
   // double multiplier = 100000;
    double speedLimit = 10.0;

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
        //set initial value for last update and current time
        lastUpdate = currentTime = Input.location.lastData.timestamp;
        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
        //debugObject.SetActive(false);
    }

    void Update()
    {
        /*
        //toggles debug text on and off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (debugObject != null)
            {
                //activate debug text
                debugObject.SetActive(!debugObject.activeSelf);
            }

        }
        */
        if (updatePosition == true)
        {
            if(updating == false && PhotonNetwork.isMasterClient)
            {
                InvokeRepeating("UpdateMasterPos", 1, 3);
                updating = true;
            }
            //debugText.text = ("Location (lat/long/alt): " + Input.location.lastData.latitude + " / " + Input.location.lastData.longitude + " / " + Input.location.lastData.altitude
             //  + " ----Horizontal accurancy/ timestamp: " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            
            if (PhotonNetwork.isMasterClient)
            {
                if(lastLat == 0)
                {
                    lastLat = myLat;
                }
                if (lastLon == 0)
                {
                    lastLon = myLon;
                }
                if (Input.location.lastData.timestamp != currentTime)
                {
                    lastUpdate = currentTime;
                    currentTime = Input.location.lastData.timestamp;
                }
                if (Input.location.lastData.longitude != masterLon)
                {
                    lastLon = masterLon;
                    masterLon = myLon = Input.location.lastData.longitude;
                }
                if (Input.location.lastData.latitude != masterLat)
                {
                    lastLat = masterLat;
                    masterLat = myLat = Input.location.lastData.latitude;
                }
                //check speed of host to be sure that host is in a car
                //if the current timestamp is later than the last timestamp
                //check the speed, then assign the last speed to the current speed
                if (currentTime > lastUpdate)
                {
                    currentSpeed = CheckSpeed();
                    //check if points should be earned
                    SpeedMaintained();
                   
                }





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

                debugText.text = ("Location (lat/long): " + myLat + " / " + myLon//your current latitude and longitude
                     + "\n Last Lat/lon: " + lastLat + " / " + lastLon           //the lat and long given on the previous update
                  + "\n timestamp: " + Input.location.lastData.timestamp         //your current timestamp
                   + "\n last time =  " + lastUpdate                             //the Timestamp given on the previous update 
                  + "\n time since last update " + (Input.location.lastData.timestamp - lastUpdate)//The time taken since the last update
                  + "\n Master (lat/long): " + masterLat + " / " + masterLon     //The lat and long of the master client
                  + "\n In range? -> " + inRange                                 //Are you in range of the master?
                  + "\n Time out of range: " + distanceTimer                     //How long have you been out of range?
                  + "\n distance from master (miles)= " + Haversine(masterLon, masterLat, myLon, myLat)//How far are you from the master?
                  + "\n speed (mph) = " + currentSpeed               //Your current speed
                  +"\n distance since last update (miles)="                             //The distance you have travelled since the last update
                  + Haversine(masterLon, masterLat, lastLon, lastLat)//Mathf.Sqrt(Mathf.Pow((masterLon - lastLon), 2) + Mathf.Pow((masterLat - lastLat), 2))
                + "\n Master? = " + PhotonNetwork.isMasterClient                 //Are you the master client?
                +"\n Earning Points? = " + earningPoints);                       //Should points be distributed?


            }
            ////////////////////////////////////////////////////////////////       
        }
    }
    //uses the Haversine formula to get proper distance using latitude and longitude
    public double Haversine(double lon1, double lat1, double lon2, double lat2)
    {
        //if the two locations are identical, skip calculations and return 0
        if(lon1 == lon2 && lat1 == lat2)
        {return 0;}

        double radlat1 = Mathf.PI * lat1 / 180;
        double radlat2 = Mathf.PI * lat2 / 180;
        double theta = lon1 - lon2;
        double radtheta = Mathf.PI * theta / 180;
        double dist = System.Math.Sin(radlat1) * System.Math.Sin(radlat2) + System.Math.Cos(radlat1) * System.Math.Cos(radlat2) * System.Math.Cos(radtheta);
        dist = System.Math.Acos(dist);
        dist = dist * 180 / Mathf.PI;
        dist = dist * 60 * 1.1515f;
        //if (unit == "K") { dist = dist * 1.609344 }
        dist = dist * 0.8684f;
        return dist;
        /*
        //radius of earth
        float R = 3961;
        float dlon = lon2 - lon1;
        float dlat = lat2 - lat1;
        float a = (Mathf.Pow((Mathf.Sin(dlat / 2)), 2)) + Mathf.Cos(lat1) * Mathf.Cos(lat2) * Mathf.Pow((Mathf.Sin(dlon / 2)) , 2);
        double c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        double d = R * c;
        return d;
        */

    }

    void OnApplicationQuit()
    {
        Input.location.Stop();
    }
    public double GetDistance()
    {
        //if master, return 0 (master is always 0 units from themself)
        if (PhotonNetwork.isMasterClient)
        { return 0; }
        //calculates distance between host and guest with longitude and latitude using distance formula
        return System.Math.Sqrt(System.Math.Pow((masterLon-myLon),2)+System.Math.Pow((masterLat-myLat),2));
    }
    //checks to see if host is in a car by comparing location over a period of time
    //uses mph
    public double CheckSpeed()
    {
        if(!PhotonNetwork.isMasterClient)
        { return 0; }
        //check distance travelled based on longitude and latitude (and altitude?) over a set period of time
        //return true if in a car, return false if not

        double distance = Haversine(masterLon, masterLat, lastLon, lastLat);// Mathf.Sqrt(Mathf.Pow((masterLon- lastLon), 2) + Mathf.Pow((masterLat-lastLat), 2));
        double hourConversion = 0.000277778;
        double time = (currentTime-lastUpdate)*hourConversion;//hourconversion changes from seconds to hours
       
        ///fix initial speed check
        /*
        if (lastLon == 0 || lastLat == 0)
        {
            distance = 0;
        }
        */
        //in mph
        double speed = (distance / time);
        
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

    public void SpeedMaintained()
    {
       
        //if master is moving over speed limit, set timer to 0
        //set earning points to true
        if ((currentSpeed ) >= speedLimit)
        {
            earningPoints = true;
            speedTimer = 0;
        }
        //if master is slower than speed limit, increment timer
        else
        {
            speedTimer += Time.deltaTime;
            //if the timer is greater than the max time out of range
            if (speedTimer > timeToEndPoints)
            {
                earningPoints = false;
            }
        }


    }
    public void UpdateMasterPos()
    {
        photonView.RPC("UpdateMaster", PhotonTargets.OthersBuffered, myLon, myLat);
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
            //distance returns in miles, convert to feet;
            //distance=5280(ftdist)
            float feetInMile = 5280;
            if((GetDistance()/ feetInMile)> variance)
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
        if (Haversine(masterLon, masterLat, myLon, myLat) > variance)
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
    */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
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
        */
        
    }
    
    [PunRPC]
    public void UpdateMaster(float lon, float lat)
    {
        
            masterLon = lon;
            masterLat = lat;
            debugText.text = ("Received Update from Master"
                + "\n MasterLon = " + masterLon
                + "\n MasterLat = " + masterLat);
        
    }

}
