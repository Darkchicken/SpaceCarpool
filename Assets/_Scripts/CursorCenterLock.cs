using UnityEngine;
using System.Collections;

public class CursorCenterLock : MonoBehaviour
{
    public float horizontalSpeed = 500f;
    public float verticalSpeed = 500f;
    public  float x;
    public  float y;

    void Update()
    {
        x = horizontalSpeed * Input.GetAxis("Mouse X");
        y = horizontalSpeed * Input.GetAxis("Mouse Y");
        transform.Translate(x,y,0);
            
    }
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;

    public float sensitivityX = 10F;
    public float sensitivityY = 10F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    public float cameraSpeedFloat;

    Transform cameraTransform;

    void Start()
    {

        cameraTransform = transform;

        if (rigidbody)
        {
            rigidbody.freezeRotation = true;
        }
    }

    void Update()
    {

        //Look direction
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            cameraTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        else if (axes == RotationAxes.MouseX)
        {
            cameraTransform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }

        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            cameraTransform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }


        //Movement direction
        if (Input.GetKey(KeyCode.W))
        {
            cameraTransform.position += cameraTransform.forward * cameraSpeedFloat * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            cameraTransform.position += -cameraTransform.forward * cameraSpeedFloat * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            cameraTransform.position += -cameraTransform.right * cameraSpeedFloat * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            cameraTransform.position += cameraTransform.right * cameraSpeedFloat * Time.deltaTime;
        }
    }

}
