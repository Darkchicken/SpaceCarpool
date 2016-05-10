using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {
    //displays the reason for game over (returned to base, out of health, out of fuel)
    public Text gameOverText;
    //displays the final score for this session
    public Text scoreText;
    //displays the leaderboard
    public Text leaderboardText;

	// Use this for initialization
	void Start ()
    {
        scoreText.text = "You gathered " + PlayFabDataStore.playerScore + " resources";
	    if(PlayFabDataStore.shipHealth <= 0)
        {
            gameOverText.text = "Your ship was damaged";
        }
        else if(PlayFabDataStore.shipFuel <= 0)
        {
            gameOverText.text = "Your ship ran out of fuel";
        }
        else
        {
            gameOverText.text = "You ended the scavenging mission";
        }
        leaderboardText.text = "LEADERBOARD:\n";
        int count = 0;
        foreach(var player in PlayFabDataStore.leaderboard)
        {

            if (player == null)
            {
                leaderboardText.text += "EMPTY\n";
            }
            else
            {
                count++;
                leaderboardText.text += player + "\n";
            }
        }
        for(int i = count; i<10;i++)
        {
            leaderboardText.text += "EMPTY\n";
        }
    }
	
	public void ReturnToMenu()
    {
        SceneManager.LoadScene("Login");
    }
}
