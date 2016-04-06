﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;
public class PhotonMatchmaker : PunBehaviour
{
    public GameObject locationHandler;
    //public Transform spawnPoint;
    //public Transform enemySpawnPoint;
    public Text debugText;
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings("0.1");
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        debugText.text = "Host a game (Your room name will be the same as your username)"
            +"\n Join a game(Type the name of the user who is hosting a game)"
             + "\n Begin game (Start the first level, must be in a room)";
        //PhotonNetwork.JoinRandomRoom();
        //PhotonNetwork.CreateRoom(null);
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        
        ///argument is room name (null to assign a random name)
        PhotonNetwork.CreateRoom(null);
        
        Debug.Log("Creating a new room!");

    }

    public override void OnJoinedRoom()
    {
        //PlayFabUserLogin.playfabUserLogin.Authentication("SUCCESS!", 2); //change the text of authentication text
        Debug.Log("Join Room Successfully!");
        Debug.Log("Room name is: "+PhotonNetwork.room);
        debugText.text = "Join Room Successfully! Room name is: " + PhotonNetwork.room
            +"\n Current longitude = "+ locationHandler.GetComponent<CheckLocation>().myLon 
            +"\n Current latitude = " + locationHandler.GetComponent<CheckLocation>().myLat;

        //Set player index for coloring
        PlayFabDataStore.laserBoltColorIndex = PhotonNetwork.playerList.Length - 1;
        Debug.Log("player size " + PhotonNetwork.playerList.Length);

        //if you are not the host
        if (!PhotonNetwork.isMasterClient)
        {
            //check if joining player is near host
            if (locationHandler.GetComponent<CheckLocation>().CompareLocationOnJoin())
            {
                Debug.Log("You are near host, join is complete");
                debugText.text = "You are near host, join is complete";
            }
            //if they are not, leave room
            else
            {
                Debug.Log("Cannot Join room, you are not near host");
                debugText.text = "Cannot Join room, you are not near host";
                PhotonNetwork.LeaveRoom();
            }
        }
        /*
        else//if you are the host
        {
            locationHandler.GetComponent<CheckLocation>().StoreMasterData();
        }
        */
        







        //Everytime a room created by a user, this part of the code has to be called to store the room name on playfab
        string[] roomName = PhotonNetwork.room.ToString().Split('\'');
        //PlayFabDataStore.currentRoomName = roomName[1]; //gets the actual room value
        Dictionary<string, string> customData = new Dictionary<string, string>();
        //customData.Add("RoomName", PlayFabDataStore.currentRoomName);
        //PlayFabApiCalls.UpdateUserData(customData);


        //GameObject player = PhotonNetwork.Instantiate("PlayerCharacter", spawnPoint.position, Quaternion.identity, 0);
        //player.GetComponent<PlayerCombatManager>().enabled = true;

    }
    void OnPlayerJoinedRoom()
    {
        if(PhotonNetwork.isMasterClient)
        {
            debugText.text = "New player joined room "+ PhotonNetwork.room;
        }

    }
   




}
