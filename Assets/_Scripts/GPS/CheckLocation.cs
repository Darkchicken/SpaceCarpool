using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckLocation : MonoBehaviour {


    public Text debugText;
    bool updatePosition = false;

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
            }
            //if this user in not the host
            else
            {
                //if the player is in a room with a host
                if (PhotonNetwork.inRoom)
                {
                    //check range, increment timer if not in range
                    TimeInRange();
                    //update your position
                    myLon = Input.location.lastData.longitude;
                    myLat = Input.location.lastData.latitude;
                    //check distance to host
                    CompareLocation();
                }


            }
        }
    }

    void OnApplicationQuit()
    {
        Input.location.Stop();
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
        if(PhotonNetwork.isMasterClient)
        {
            Debug.Log("No need to compare master position to itself");
            inRange = true;
            return;
        }
        //how far off a distance can be before a player is dropped
        float variance = 0.0001f;
        float lonDiff = masterLon - myLon;
        float latDiff = masterLat - myLat;
        if(lonDiff > variance || lonDiff < -variance || latDiff > variance || latDiff<-variance )
        {
            inRange = false;
            return;
        }
        else
        {
            inRange = true;
            return;
        }

        
    }
    public bool CompareLocationOnJoin()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("No need to compare master position to itself");
            return true;
        }
        //how far off a distance can be before a player is dropped
        float variance = 0.0001f;
        float lonDiff = masterLon - myLon;
        float latDiff = masterLat - myLat;
        if (lonDiff > variance || lonDiff < -variance || latDiff > variance || latDiff < -variance)
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
