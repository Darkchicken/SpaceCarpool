using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
/*
Transition from the opening title panel to the login page
*/
public class OpeningTransition : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //jumps to login page in 3 seconds
        Invoke("JumpToLogin", 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void JumpToLogin()
    {
        //add a fade?
        SceneManager.LoadScene(1);
    }
}
