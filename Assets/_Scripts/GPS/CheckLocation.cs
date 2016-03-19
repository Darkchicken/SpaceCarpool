using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckLocation : MonoBehaviour {


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

    public float myLon;
    public float myLat;
    public float masterLon;
    public float masterLat;
    IEnumerator Start()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

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

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }

    void Update()
    {
       
        if (updatePosition == true)
        {
            //debugText.text = ("Location (lat/long/alt): " + Input.location.lastData.latitude + " / " + Input.location.lastData.longitude + " / " + Input.location.lastData.altitude
             //  + " ----Horizontal accurancy/ timestamp: " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            
            if (PhotonNetwork.isMasterClient)
            {
                //check speed of host to be sure that host is in a car
                CheckSpeed();
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
            if (SceneManager.GetActiveScene().name == "TestGPS")
            {
                debugText.text = ("Location (lat/long): " + myLat + " / " + myLon
                  + "\n timestamp: " + Input.location.lastData.timestamp
                  + "\n Master (lat/long): " + masterLat + " / " + masterLon
                  + "\n In range? -> " + inRange
                  + "\n Time out of range: " + distanceTimer
                  + "\n distance = " + GetDistance());
                 
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
    //checks to see if host is in a car by comparing location over a period of time
    public bool CheckSpeed()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.Log("Only the host should be checked for speed");
            return true;
        }
        //check distance travelled based on longitude and latitude (and altitude?) over a set period of time
        //return true if in a car, return false if not


        return false;
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
   
}
