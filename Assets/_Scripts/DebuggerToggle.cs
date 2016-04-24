using UnityEngine;
using System.Collections;

public class DebuggerToggle : MonoBehaviour {

    public GameObject debugObject;
	
	// Update is called once per frame
	void Update ()
    {
	//toggles debug text on and off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (debugObject != null)
            {
                //activate debug text
                debugObject.SetActive(!debugObject.activeSelf);
            }

        }
	}
}
