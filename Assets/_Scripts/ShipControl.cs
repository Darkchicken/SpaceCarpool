using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShipControl : MonoBehaviour {

	void Start ()
    {
        StartCoroutine(FuelUse());
    }

    IEnumerator FuelUse()
    {
        
        yield return new WaitForSeconds(5);

        PlayFabDataStore.shipFuel -= 3;
        GameHUDManager.gameHudManager.HudUpdate();
        if(PlayFabDataStore.shipFuel <= 0)
        {
            GameOver();
        }

        StartCoroutine(FuelUse());
    }

    public void TakeDamage(int damage)
    {
        GameHUDManager.gameHudManager.HudUpdate();

        if (PlayFabDataStore.shipHealth - damage > 0)
        {
            Debug.Log("Ship took damage: " + damage);
            PlayFabDataStore.shipHealth -= damage;
        }
        else
        {
            GameOver();
        }
        
    }
    void GameOver()
    {
        //GameOVER!!!
        PlayFabDataStore.allTimeScore += PlayFabDataStore.playerScore;
        PlayFabApiCalls.UpdateUserStatistic("AllTime");
        if (PhotonNetwork.isMasterClient)
        {
            GameManager.gameManager.isGameStarted = false;
            Debug.Log("Game Over");
        }
        SceneManager.LoadScene("EndingScene");
    }
}
