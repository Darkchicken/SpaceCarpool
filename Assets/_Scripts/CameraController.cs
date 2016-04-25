using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//attach this script to the camera to rotate the camera angle based on the accelerometer
public class CameraController : MonoBehaviour {

    Vector3 initialPosition;
    Quaternion initialRotation;

	public int speed;

    public float minX = -30F;
    public float maxX = 30F;
    public float minY = -60F;
    public float maxY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

	private Camera playerCamera;

    bool gyroActive = true;

    void Start()
    {
        GameObject.Find("GameHUD").GetComponent<GameHUDManager>().SetPlayer(this.gameObject);
        playerCamera = Camera.main;
        playerCamera.gameObject.transform.position = transform.position;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        Input.compensateSensors = true;
        Input.gyro.enabled = true;
        //assigns the click function for the camera reset button at runtime
        GameObject.Find("Camera Reset Button").GetComponent<Button>().onClick.AddListener(() => ResetView());
    }

    void FixedUpdate()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        */
        if (Application.isMobilePlatform && gyroActive)
        {
            rotationX += -Input.gyro.rotationRateUnbiased.x;
            rotationY += -Input.gyro.rotationRateUnbiased.y;

            rotationX = Mathf.Clamp(rotationX, minX, maxX);
            rotationY = Mathf.Clamp(rotationY, minY, maxY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.right);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.up);

            transform.rotation = initialRotation * xQuaternion * yQuaternion;
            
            //transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);//Input.gyro.rotationRateUnbiased.z
            playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, transform.rotation, speed * Time.deltaTime);
        }
    }

    void Update () {

        /*
        Debug.Log(Input.acceleration);
		// up/down and left/right movement
		// change where acc.y = 0, control about 20 degrees of movement
		//transform.Rotate (Input.acceleration.y * Time.deltaTime * speed, Input.acceleration.x * Time.deltaTime * speed, 0);
        //

        float xMovement = 0;
        float yMovement = 0;
        ///additional code by josh
        
        if (Input.acceleration.x > 0.3) 
        {
            //1 is for input direction
           xMovement = 1 * speed * Time.deltaTime;
        }
        if(Input.acceleration.x < -0.3)
        {
            xMovement = -1 * speed * Time.deltaTime;
        }
        
        if (Input.acceleration.y < 0)
        {
            //1 is for input direction
            yMovement = 1 * speed * Time.deltaTime;
        }
        if (Input.acceleration.y < -0.9)
        {
            yMovement = 1 * speed * Time.deltaTime;
        }
        
        //float xMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //float yMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Rotate(yMovement, xMovement, 0);
        playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, transform.rotation, speed * Time.deltaTime);
        */
    }
    public void ResetView()
    {
        gyroActive = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rotationX = 0F;
        rotationY = 0F;
        playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, transform.rotation, speed * Time.deltaTime);
        gyroActive = true;
    }
}