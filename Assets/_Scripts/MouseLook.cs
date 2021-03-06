﻿using UnityEngine;
using System.Collections;


public class MouseLook : MonoBehaviour
{
    public Camera playerCamera;
    int speed = 10;

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
    bool joystick = false;

    void Start()
    {
        playerCamera = Camera.main;

        originalRotation = transform.localRotation;
        if (PlayerPrefs.GetInt("MotionActivate") == 1)
        {
            joystick = false;
        }
        else
        {
            joystick = true;
        }


    }

    void Update()
    {

        if (joystick == false)
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
            else
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

            playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, transform.rotation, speed * Time.deltaTime);
        }
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
