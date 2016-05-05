using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabDataStore : MonoBehaviour
{

    public static string sessionTicket;
    public static string playFabId;
    public static string userName;
    public static float playerLatitude;
    public static float playerLongitude;
    public static string masterClientUserName;
    public static string masterClientPlayFabId;
    public static float masterClientLatitude;
    public static float masterClientLongitude;

    public static int laserBoltColorIndex;

    public static int shipFuel;
    public static int shipFuelMax = 100;
    public static int shipHealth;
    public static int shipHealthMax = 100;

    public static int playerScore = 0;
    public static int allTimeScore = 0;

    public static List<string> leaderboard = new List<string>(); 

    /*
    Dictionary<string, string> customData = new Dictionary<string, string>(); //create a dictionary for the data
    string locationString = playerLatitude + "#" + playerLongitude;
    customData.Add("Location", locationString); //locationString has to be this way: Latitude#Longtitude
    PlayFabApiCalls.UpdateUserLocation(customData); // then call this function and send the dictionary

    */



}
