using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUDManager : MonoBehaviour {

    public AudioSource resourceSound;
    public static GameHUDManager gameHudManager;
    public Toggle weaponToggle;
    public Text scoreText;
    public Image crosshairBlue;
    public Image crosshairRed;

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
            weaponToggle.isOn = !weaponToggle.isOn;

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
            crosshairBlue.enabled = true;
            crosshairRed.enabled = false;
        }
        else
        {
            player.GetComponent<PlayerCombatManager>().SetWeapon(true);
            crosshairBlue.enabled = false;
            crosshairRed.enabled = true;
        }
    }

    public void SetScore()
    {
        resourceSound.Play();
        scoreText.text = "Score: " + PlayFabDataStore.playerScore;
    }
}
