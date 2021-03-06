﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon;
using UnityEngine.SceneManagement;

public class PhotonCalls : PunBehaviour
{

    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            string spawnpointName = "SpawnPoint" + (PlayFabDataStore.laserBoltColorIndex + 1);
            GameObject spawnPoint = GameObject.Find(spawnpointName);
            //instantiate player on all clients
            Debug.Log(spawnpointName);
            GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.transform.position, Quaternion.identity, 0);
            if (!PhotonNetwork.isMasterClient && player.GetComponent<PhotonView>().isMine)
            {
                //enable scripts only for the controlling player
                player.GetComponent<PlayerCombatManager>().enabled = true;
                player.GetComponent<CameraController>().enabled = true;
                player.GetComponent<CheckLocation>().enabled = true;
#if UNITY_STANDALONE

                player.GetComponent<MouseLook>().enabled = true;

#endif

#if UNITY_EDITOR

                player.GetComponent<MouseLook>().enabled = true;

#endif
                
            }
        }
    }
        
    public static void NewRoom()
    {

        PhotonNetwork.CreateRoom(null);
        
    }
    //exits the current room
    public static void LeaveRoom()
    {
        
        PhotonNetwork.LeaveRoom();
    }
    //exits the current room, but also preps to join a friends room
    public static void JoinFriendRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    //when the player leaves their current room, reenter the lobby
    public override void OnLeftRoom()
    {
        PhotonNetwork.JoinLobby();
    }
    //upon reaching the lobby, join a random room 
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Login");
    }
   
    //if the player fails to join a random room
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        Debug.Log("Creating a new room!");
        ///argument is room name (null to assign a random name)
        PhotonNetwork.CreateRoom("Tester");

       
    }

    //upon joining a new room, output the room name
    public override void OnJoinedRoom()
    {
        Debug.Log("Onjoined room");
        PlayFabDataStore.laserBoltColorIndex = PhotonNetwork.playerList.Length - 1;
        GameManager.gameManager.InitializeThePlayer();
    }


}

