using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickControls : MonoBehaviour {

    public Camera playerCamera;
    int speed = 10;

    bool JoyStick = true;
    int JoyStickCounter = 1;
    public float JoyStickSensitivity = 5f;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    float minimumX = -70F;
    float maximumX = 70F;

    float minimumY = -60F;
    float maximumY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

    Quaternion originalRotation;
    // Use this for initialization
    void Start ()
    {
        playerCamera = Camera.main;

        originalRotation = transform.localRotation;

        if (PlayerPrefs.GetInt("MotionActivate") == 1)
        {
            JoyStick = false;
        }
        else
        {
            JoyStick = true;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (JoyStick == true)
        {

            if (axes == RotationAxes.MouseXAndY)
            {
                // Read the mouse input axis
                rotationX += CrossPlatformInputManager.GetAxis("Horizontal") * JoyStickSensitivity;
                rotationY += CrossPlatformInputManager.GetAxis("Vertical") * JoyStickSensitivity;
                Debug.Log(rotationX+"," +rotationY);

                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                //Debug.Log("X - Q :" + xQuaternion);
                //Debug.Log("Y - Q :" + yQuaternion);

                playerCamera.transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                /*if((xQuaternion.y >= -0.5f && xQuaternion.y <= 0.5f) && (yQuaternion.x >= -0.3f && yQuaternion.x <= 0.3f))
                {
                    transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                }*/

            }

            else if (axes == RotationAxes.MouseX)
            {
                rotationX += CrossPlatformInputManager.GetAxis("Horizontal") * JoyStickSensitivity;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);

                playerCamera.transform.localRotation = originalRotation * xQuaternion;
                /*if (xQuaternion.y >= -0.5f && xQuaternion.y <= 0.5f)
                {
                    transform.localRotation = originalRotation * xQuaternion;
                }*/

            }
            else
            {
                rotationY += CrossPlatformInputManager.GetAxis("Vertical") * JoyStickSensitivity;
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);

                playerCamera.transform.localRotation = originalRotation * yQuaternion;
                /*if (yQuaternion.x >= -0.3f && yQuaternion.x <= 0.3f)
                {
                    transform.localRotation = originalRotation * yQuaternion;
                }*/

            }
        }   //end if statement joystick = true
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
