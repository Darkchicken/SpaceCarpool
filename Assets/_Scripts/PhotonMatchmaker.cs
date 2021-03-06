﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;
public class PhotonMatchmaker : PunBehaviour
{
    public GameObject locationHandler;

    public Text debugText;
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings("0.1");
    }


    public override void OnJoinedLobby()
    {
        //sets player name in game to be the same as username to login
        PhotonNetwork.player.name = PlayFabDataStore.userName;
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
        Debug.Log("Join Room Successfully!");
        Debug.Log("Room name is: "+PhotonNetwork.room);
        debugText.text = "Join Room Successfully! Room name is: " + PhotonNetwork.room
            +"\n Current longitude = "+ locationHandler.GetComponent<CheckLocation>().myLon 
            +"\n Current latitude = " + locationHandler.GetComponent<CheckLocation>().myLat;

        //if you are not the host
        if (!PhotonNetwork.isMasterClient)
        {
            //check if joining player is near host
            if (locationHandler.GetComponent<CheckLocation>().CompareLocationOnJoin())
            {
                Debug.Log("You are near host, join is complete");
                debugText.text = "You are near host, join is complete";
                BeginGame();
            }
            //if they are not, leave room
            else
            {
                Debug.Log("Cannot Join room, you are not near host");
                debugText.text = "Cannot Join room, you are not near host";
                PhotonNetwork.LeaveRoom();
            }
        }
        
        else//if you are the host
        {
            BeginGame();
        }
        
        







        //Everytime a room created by a user, this part of the code has to be called to store the room name on playfab
        string[] roomName = PhotonNetwork.room.ToString().Split('\'');
        //PlayFabDataStore.currentRoomName = roomName[1]; //gets the actual room value
        Dictionary<string, string> customData = new Dictionary<string, string>();
        //customData.Add("RoomName", PlayFabDataStore.currentRoomName);
        //PlayFabApiCalls.UpdateUserData(customData);


        //GameObject player = PhotonNetwork.Instantiate("PlayerCharacter", spawnPoint.position, Quaternion.identity, 0);
        //player.GetComponent<PlayerCombatManager>().enabled = true;

    }
    public void BeginGame()
    {
        if (PhotonNetwork.room != null)
        {
            Debug.Log("Starting Game");
            debugText.text = "Starting game in room named: " + PhotonNetwork.room.name;
            PhotonNetwork.LoadLevel("TestSpawning");
        }
        else
        {
            Debug.Log("Cannot begin, must host or join a room first");
            debugText.text = "Cannot begin, must host or join a room first";
        }
    }
    void OnPlayerJoinedRoom()
    {
        if(PhotonNetwork.isMasterClient)
        {
            debugText.text = "New player joined room "+ PhotonNetwork.room;
        }

    }
   




}
