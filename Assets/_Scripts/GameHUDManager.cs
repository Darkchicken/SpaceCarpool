using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUDManager : MonoBehaviour {

    public static GameHUDManager gameHudManager;
    public Toggle weaponToggle;
    public Text scoreText;

    private GameObject player;


    void Start()
    {
        gameHudManager = this;
        //player = GameObject.FindGameObjectWithTag("Player");
        SetScore();
    }
#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SelectWeapon();
        }
    }
#endif
    public void SetPlayer(GameObject p)
    {
        player = p;
    }
    public void SelectWeapon()
    {
        if(weaponToggle.isOn) // true is beam
        {
            player.GetComponent<PlayerCombatManager>().SetWeapon(false);
        }
        else
        {
            player.GetComponent<PlayerCombatManager>().SetWeapon(true);
        }
    }

    public void SetScore()
    {
        scoreText.text = "Score: " + PlayFabDataStore.playerScore;
    }
}
