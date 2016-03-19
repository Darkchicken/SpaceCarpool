using UnityEngine;
using System.Collections;

public class MoveObjects : MonoBehaviour {

    
    private Transform cameraTransform;
    private float startTime;
    private Rigidbody rb;
    private Vector3 endPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        startTime = Time.deltaTime;
        endPosition = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z - 2000);
    }

	void FixedUpdate ()
    {
        transform.position = Vector3.Lerp(transform.position, endPosition, (Time.time - startTime) / 10000);
	}
	
}
