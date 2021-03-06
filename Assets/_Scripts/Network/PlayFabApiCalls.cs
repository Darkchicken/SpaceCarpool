﻿using UnityEngine;
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
            PlayFabUserLogin.playFabUserLogin.login.gameObject.SetActive(false);
            PlayFabUserLogin.playFabUserLogin.mainMenu.gameObject.SetActive(true);
            PlayFabDataStore.playFabId = result.PlayFabId;
            PlayFabDataStore.userName = username;
            PlayFabDataStore.sessionTicket = result.SessionTicket;
            UpdateUserStatistic("AllTime");
            GetPhotonToken();
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
            PlayFabDataStore.userName = username;
            PlayFabUserRegister.playFabUserRegister.gameObject.SetActive(false);
            PlayFabUserRegister.playFabUserRegister.mainMenu.gameObject.SetActive(true);
            Debug.Log("New Account Created!");
            //UpdateUserStatistic("AllTime");
            GetPhotonToken();
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

    //Get Photon Token from playfab
    public static void GetPhotonToken()
    {
        var request = new GetPhotonAuthenticationTokenRequest();
        {
            request.PhotonApplicationId = "ee6881e0-06d0-4b7c-b552-a4087cd14926".Trim();
        }

        PlayFabClientAPI.GetPhotonAuthenticationToken(request, (result) =>
        {
            string photonToken = result.PhotonCustomAuthenticationToken;
            Debug.Log(string.Format("Yay, logged in in session token: {0}", photonToken));
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
            PhotonNetwork.AuthValues.AddAuthParameter("username", PlayFabDataStore.playFabId);
            PhotonNetwork.AuthValues.AddAuthParameter("Token", result.PhotonCustomAuthenticationToken);
            PhotonNetwork.AuthValues.UserId = PlayFabDataStore.playFabId;
            PhotonNetwork.ConnectUsingSettings("1.0");
            //make sure all players are synced to same scene in same room
            PhotonNetwork.automaticallySyncScene = true;
            GetUserStatistic();
        }, (error) =>
        {
            Debug.Log("Photon Connection Failed! " + error.ErrorMessage.ToString()); 
        });
    }

    public static void UpdateUserLocation(Dictionary<string, string> data)
    {
        var request = new UpdateUserDataRequest()
        {
            Data = data,
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) =>
        {
            Debug.Log("User Data Updated");
        },
        (error) =>
        {
            Debug.Log("User Data Can't Updated");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Gets specific user's Location
    public static void GetUserLocation(string playFabId)
    {
        var request = new GetUserDataRequest()
        {
            PlayFabId = playFabId
        };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            string[] location = result.Data["Location"].Value.Split('#');
            PlayFabDataStore.masterClientLatitude = float.Parse(location[0]);
            PlayFabDataStore.masterClientLongitude = float.Parse(location[1]);
        },
        (error) =>
        {
            Debug.Log("Can't get Location");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Get Player Statistics
    public static void GetUserStatistic()
    {
        var request = new GetUserStatisticsRequest()
        {
            
        };

        PlayFabClientAPI.GetUserStatistics(request, (result) =>
        {
            foreach(var statistic in result.UserStatistics)
            {
                if(statistic.Key.ToString() == "AllTime")
                {
                    PlayFabDataStore.allTimeScore = statistic.Value;
                }
            }
            Debug.Log("Statistic Retrieved " + PlayFabDataStore.allTimeScore);
        },
        (error) =>
        {
            Debug.Log("Can't get user statistic");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Updates Player Statistics
    public static void UpdateUserStatistic(string statisticName)
    {
        Dictionary<string, int> statistics = new Dictionary<string, int>();
        statistics.Add(statisticName, PlayFabDataStore.allTimeScore);
        var request = new UpdateUserStatisticsRequest()
        {
            UserStatistics = statistics
        };

        PlayFabClientAPI.UpdateUserStatistics(request, (result) =>
        {
            Debug.Log("Statistic Updated");
        },
        (error) =>
        {
            Debug.Log("Can't update statistic");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //GetLeaderboard
    public static void GetLeaderboard(string statisticName)
    {
        var request = new GetLeaderboardRequest()
        {
            StatisticName = statisticName,
            StartPosition = 1
        };

        PlayFabClientAPI.GetLeaderboard(request, (result) =>
        {
            Debug.Log("Leaderboard Retrieved");
            PlayFabDataStore.leaderboard.Clear();
            foreach(var player in result.Leaderboard)
            {
                PlayFabDataStore.leaderboard.Add(player.DisplayName);
            }
        },
        (error) =>
        {
            Debug.Log("Can't get Leaderboard");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

}
