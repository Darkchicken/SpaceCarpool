using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabUserLogin : MonoBehaviour
{
   
    public InputField loginUsernameField;
    public InputField loginPasswordField;
    public Text errorText;
    //output prompts and errors to user
    public Text debugText;

    public InputField joinInputField;

    public Canvas createAccount;
    public Canvas mainMenu;

    public static PlayFabUserLogin playFabUserLogin;

    void Awake()
    {
        playFabUserLogin = this;
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
        debugText.text = "Creating room: " + PlayFabDataStore.userName;
        //Debug.Log("Created Room: "+ PhotonNetwork.room.name);
        //create a room with same name as host
        if(PhotonNetwork.CreateRoom(PlayFabDataStore.userName))
        {
            debugText.text = "Room with name: "+PhotonNetwork.room+" created!";
           
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
            PhotonNetwork.LoadLevel("Test");
        }
        else
        {
            Debug.Log("Cannot begin, must host or join a room first");
            debugText.text = "Cannot begin, must host or join a room first";
        }
    }
   
}
