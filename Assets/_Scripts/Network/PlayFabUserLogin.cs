using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabUserLogin : MonoBehaviour
{

    public GameObject locationHandler;
    public Text hostInfo;
    public GameObject hostButton;
    public GameObject beginButton;
    public GameObject joinButton;
    public GameObject joinTextField;
    public GameObject backButton;
    public GameObject instructionPanel;
    public GameObject instructionsButton;
    bool waitingForPlayers = false;

    public InputField loginUsernameField;
    public InputField loginPasswordField;
    public Text errorText;
    //output prompts and errors to user
    public Text debugText;

    public InputField joinInputField;

    public Canvas login;
    public Canvas createAccount;
    public Canvas mainMenu;

    public static PlayFabUserLogin playFabUserLogin;

    void Awake()
    {
        playFabUserLogin = this;
       
    }
    void Update()
    {
        if (waitingForPlayers)
        {
            //for testing///////////////////////////////////
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                beginButton.SetActive(true);
            }
           //////////////////////////////////////////////////
           
            if (PhotonNetwork.otherPlayers.Length > 0)
            {
                string playerList = "Ready to fly:\n";
                foreach (PhotonPlayer player in PhotonNetwork.otherPlayers)
                {
                    playerList += player.name + "\n";
                }
                hostInfo.text = playerList;
                if (!beginButton.activeSelf && PhotonNetwork.countOfPlayers > 1)
                {
                    beginButton.SetActive(true);
                }
                //Debug.Log(playerList);
            }
        }
    }

    public void Login()
    {
        PlayFabApiCalls.PlayFabLogin(loginUsernameField.text, loginPasswordField.text);
    }

    public void Register()
    {
        errorText.enabled = false;
        createAccount.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Host()
    {
        //if the player is already in a room, prevent hosting
        if(PhotonNetwork.inRoom || !PhotonNetwork.insideLobby)
        {
            debugText.text = "Cannot create room, either already in a room or not in lobby";
            return;
        }
        debugText.text = "Creating room: " + PlayFabDataStore.userName;
        //Debug.Log("Created Room: "+ PhotonNetwork.room.name);
        //create a room with same name as host
        if(PhotonNetwork.CreateRoom(PlayFabDataStore.userName))
        {
            debugText.text = "Room with name: "+PhotonNetwork.room+" created!";
            //Set player index for coloring
            Debug.Log("player size " + PhotonNetwork.playerList.Length);
            PlayFabDataStore.laserBoltColorIndex = PhotonNetwork.playerList.Length - 1;
            hostInfo.text = "Waiting for teammates...";
            instructionsButton.SetActive(false);
            hostButton.SetActive(false);
            joinButton.SetActive(false);
            joinTextField.SetActive(false);
            backButton.SetActive(true);
            waitingForPlayers = true;
          
           



        }
        else
        {
            debugText.text = "Failed to create room";
        }
    }

    public void Join()
    {

        if(joinInputField.text != null)
        {
            debugText.text = "Attempting to join room: " + joinInputField.text;
            Debug.Log("Attempting to join room: "+ joinInputField.text);
            PhotonNetwork.JoinRoom(joinInputField.text);
            


        }
        else
        {
            Debug.Log("Input a room name in the text field");
            debugText.text = "Input a room name in the text field";
        }
        //use this to join a named room
        //PhotonNetwork.JoinRoom();
        //PhotonNetwork.JoinRandomRoom();
        //
    }
    public void BeginGame()
    {
        if (PhotonNetwork.room != null)
        {
            Debug.Log("Starting Game");
            debugText.text = "Starting game in room named: "+ PhotonNetwork.room.name;
            PhotonNetwork.LoadLevel("TestSpawning");
        }
        else
        {
            Debug.Log("Cannot begin, must host or join a room first");
            debugText.text = "Cannot begin, must host or join a room first";
        }
    }
    public void GoBack()
    {
        if (PhotonNetwork.room != null)
        {
            PhotonNetwork.LeaveRoom();
        }
        waitingForPlayers = false;
        instructionsButton.SetActive(true); ;
        instructionPanel.SetActive(false);
        hostButton.SetActive(true);
        joinButton.SetActive(true);
        joinTextField.SetActive(true);
        beginButton.SetActive(false);
        backButton.SetActive(false);
        hostInfo.text = "";

    }
    public void Instructions()
    {
        instructionPanel.SetActive(true);

    }
   
 

}
