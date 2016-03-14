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
        PhotonNetwork.CreateRoom(null);
        Debug.Log("Created Room: "+ PhotonNetwork.room.name);
    }
    public void Join()
    {
        //use this to join a named room
        //PhotonNetwork.JoinRoom();
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Joining Room");
    }
    public void BeginGame()
    {
        Debug.Log("Starting Game");
        PhotonNetwork.LoadLevel("Test");
    }

}
