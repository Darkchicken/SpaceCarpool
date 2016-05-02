using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MouseLook : MonoBehaviour
{
    public Camera playerCamera;
    int speed = 10;

    bool JoyStick = true;
    int JoyStickCounter = 1;
    public float JoyStickSensitivity = 5f;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

    Quaternion originalRotation;

    void Start()
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

    void Update()
    {
        Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")) * sensitivityX;

        if(Input.GetKeyDown("tab"))
        {
            if(JoyStickCounter == 0)
            {
                JoyStick = true;
                JoyStickCounter = 1;
            }

            else
            {
                JoyStick = false;
                JoyStickCounter = 0;
            }
            
        }

        if (JoyStick == true)
        {

            if (axes == RotationAxes.MouseXAndY)
            {
                // Read the mouse input axis
                rotationX += CrossPlatformInputManager.GetAxis("Horizontal") * JoyStickSensitivity;
                rotationY += CrossPlatformInputManager.GetAxis("Vertical") * JoyStickSensitivity;

                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                //Debug.Log("X - Q :" + xQuaternion);
                //Debug.Log("Y - Q :" + yQuaternion);

                transform.localRotation = originalRotation * xQuaternion * yQuaternion;
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

                transform.localRotation = originalRotation * xQuaternion;
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

                transform.localRotation = originalRotation * yQuaternion;
                /*if (yQuaternion.x >= -0.3f && yQuaternion.x <= 0.3f)
                {
                    transform.localRotation = originalRotation * yQuaternion;
                }*/

            }
        }   //end if statement joystick = true

        /*--------------------------------------------------------------------------------------------------------------
                GyroControls
           -------------------------------------------------------------------------------------------------------------
        */
        if (JoyStick == false && !Application.isMobilePlatform)
        {

            if (axes == RotationAxes.MouseXAndY)
            {
                // Read the mouse input axis
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                //Debug.Log("X - Q :" + xQuaternion);
                //Debug.Log("Y - Q :" + yQuaternion);

                transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                /*if((xQuaternion.y >= -0.5f && xQuaternion.y <= 0.5f) && (yQuaternion.x >= -0.3f && yQuaternion.x <= 0.3f))
                {
                    transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                }*/

            }
            else if (axes == RotationAxes.MouseX)
            {
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);

                transform.localRotation = originalRotation * xQuaternion;
                /*if (xQuaternion.y >= -0.5f && xQuaternion.y <= 0.5f)
                {
                    transform.localRotation = originalRotation * xQuaternion;
                }*/

            }
            else if (JoyStick == false)
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);

                transform.localRotation = originalRotation * yQuaternion;
                /*if (yQuaternion.x >= -0.3f && yQuaternion.x <= 0.3f)
                {
                    transform.localRotation = originalRotation * yQuaternion;
                }*/

            }
        }  // end if statement joystick = false
        playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, transform.rotation, speed * Time.deltaTime);
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
