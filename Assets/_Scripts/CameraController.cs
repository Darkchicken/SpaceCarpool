using UnityEngine;
using System.Collections;

//attach this script to the camera to rotate the camera angle based on the accelerometer
public class CameraController : MonoBehaviour {

	void Update () {
		//camera rotating side to side 
		transform.Rotate (0, Input.acceleration.x, 0);

		//Uncomment the following line for possible up/down and left/right movement
		//transform.Rotate (-Input.acceleration.y / 2, Input.acceleration.x, 0);
	}
}