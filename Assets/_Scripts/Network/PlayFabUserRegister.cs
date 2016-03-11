using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabUserRegister : MonoBehaviour
{

    public InputField registerUsernameField;
    public InputField registerPasswordField;
    public InputField registerEmailField;
    public Text errorText;

    public Canvas login;

    public static PlayFabUserRegister playFabUserRegister;

    void Awake()
    {
        playFabUserRegister = this;
    }


    public void Register()
    {
        PlayFabApiCalls.PlayFabRegister(registerUsernameField.text, registerPasswordField.text, registerEmailField.text);
    }
}
