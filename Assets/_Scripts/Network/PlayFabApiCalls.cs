using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabApiCalls : MonoBehaviour {

    //Login to playfab/game
    public static void PlayFabLogin(string username, string password)
    {
        var request = new LoginWithPlayFabRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Username = username,
            Password = password
        };

        PlayFabClientAPI.LoginWithPlayFab(request, (result) =>
        {
            Debug.Log("Login Successful!");
            PlayFabDataStore.playFabId = result.PlayFabId;
            PlayFabDataStore.sessionTicket = result.SessionTicket;
        }, (error) =>
        {
            Debug.Log("Login Failed!");
            PlayFabUserLogin.playFabUserLogin.errorText.text = error.ErrorMessage.ToString();
        });
    }

    public static void PlayFabRegister(string username, string password, string email)
    {
        var request = new RegisterPlayFabUserRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Username = username,
            Password = password,
            Email = email
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, (result) =>
        {
            PlayFabDataStore.playFabId = result.PlayFabId;
            PlayFabDataStore.sessionTicket = result.SessionTicket;
            Debug.Log("New Account Created!");
        }, (error) =>
        {
            Debug.Log("New Account Creation Failed!");
            PlayFabUserRegister.playFabUserRegister.errorText.text = error.ErrorMessage.ToString();
        });
    }

    //Access the newest version of cloud script
    public static void PlayFabInitialize()
    {
        var request = new GetCloudScriptUrlRequest()
        {
            Testing = false
        };

        PlayFabClientAPI.GetCloudScriptUrl(request, (result) =>
        {
            Debug.Log("URL is set");

        },
        (error) =>
        {
            Debug.Log("Failed to retrieve Cloud Script URL");
        });
    }
}
