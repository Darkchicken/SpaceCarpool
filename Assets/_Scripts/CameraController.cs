using UnityEngine;
using System.Collections;

//attach this script to the camera to rotate the camera angle based on the accelerometer
public class CameraController : MonoBehaviour {
	
	public int speed;
	public Camera camera;

	void Update () {
		// up/down and left/right movement
		// change where acc.y = 0, control about 20 degrees of movement
		transform.Rotate (Input.acceleration.y * Time.deltaTime * speed, Input.acceleration.x * Time.deltaTime * speed, 0);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, transform.rotation, speed * Time.deltaTime);
	}
}