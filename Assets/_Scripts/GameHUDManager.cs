using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUDManager : MonoBehaviour {

    delegate void HUDUpdateDelegate();
    HUDUpdateDelegate hudUpdateDelegate;


   
    public static GameHUDManager gameHudManager;
    public Toggle weaponToggle;
    public Text scoreText;
    public Text scavengerList;
    public Image healthImage;
    public Image fuelImage;
    public Image crosshairBlue;
    public Image crosshairRed;
    public GameObject pilotPanel;

    private GameObject player;

    


    void Start()
    {
        gameHudManager = this;
        //player = GameObject.FindGameObjectWithTag("Player");
        hudUpdateDelegate += SetScore;
        hudUpdateDelegate += SetHealth;
        hudUpdateDelegate += SetFuel;

        pilotPanel.SetActive(true);
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                weaponToggle.isOn = !weaponToggle.isOn;

            }
#endif

            string playerList = "";
            foreach (PhotonPlayer player in PhotonNetwork.otherPlayers)
            {
                playerList += player.name + "\n";
            }
            scavengerList.text = playerList;
            ///DEBUG CODE, REMOVE LATER
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                /////////////////////////////////////////////////////
            }

        }
    }
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

    public void StartButtonClicked()
    {
        
        GameObject player = GameObject.Find("Player(Clone)");
        pilotPanel.SetActive(false);
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

        GameManager.gameManager.isGameStarted = true;
    }
}
