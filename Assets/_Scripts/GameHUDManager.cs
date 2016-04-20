using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUDManager : MonoBehaviour {

    delegate void HUDUpdateDelegate();
    HUDUpdateDelegate hudUpdateDelegate;


   
    public static GameHUDManager gameHudManager;
    public Toggle weaponToggle;
    public Text scoreText;
    public Image healthImage;
    public Image fuelImage;
    public Image crosshairBlue;
    public Image crosshairRed;

    private GameObject player;

    


    void Start()
    {
        gameHudManager = this;
        //player = GameObject.FindGameObjectWithTag("Player");
        hudUpdateDelegate += SetScore;
        hudUpdateDelegate += SetHealth;
        hudUpdateDelegate += SetFuel;
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

    public void HudUpdate()
    {
        hudUpdateDelegate();
    }

    void SetScore()
    {
        
        scoreText.text = "Score: " + PlayFabDataStore.playerScore;
    }

    void SetHealth()
    {
        healthImage.fillAmount = (float)PlayFabDataStore.shipHealth / (float)PlayFabDataStore.shipHealthMax;
    }

    void SetFuel()
    {
        fuelImage.fillAmount = (float)PlayFabDataStore.shipFuel / (float)PlayFabDataStore.shipFuelMax;
    }
}
