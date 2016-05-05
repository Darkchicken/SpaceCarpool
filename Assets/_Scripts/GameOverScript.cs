using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {
    //displays the reason for game over (returned to base, out of health, out of fuel)
    public Text gameOverText;
    //displays the final score for this session
    public Text scoreText;

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
        gameOverText.text += "\nLEADERBOARD:\n";
        int count = 0;
        foreach(var player in PlayFabDataStore.leaderboard)
        {
            if (player == null)
            {
                gameOverText.text += "EMPTY\n";
            }
            else
            {
                count++;
                gameOverText.text += player + "\n";
            }
        }
        for(int i = count; i<10;i++)
        {
            gameOverText.text += "EMPTY\n";
        }
    }
	
	public void ReturnToMenu()
    {
        SceneManager.LoadScene("Login");
    }
}
