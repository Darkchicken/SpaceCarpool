using UnityEngine;
using System.Collections;

public class MoveObjects : MonoBehaviour {

    public float speed;
    private BoxCollider destinationArea;
    private Rigidbody objectRigidBody;
    private float startTime;
    private Vector3 colliderSize;
    private Vector3 endPosition;
    private Vector3 direction;

    void Start()
    {
        startTime = Time.deltaTime;
        objectRigidBody = GetComponent<Rigidbody>();
        destinationArea = GameManager.gameManager.destinationArea;
        colliderSize = destinationArea.GetComponent<BoxCollider>().size;
        endPosition = new Vector3(Random.Range(destinationArea.transform.position.x - destinationArea.size.x / 2, destinationArea.transform.position.x + destinationArea.size.x / 2),
            Random.Range(destinationArea.transform.position.y - destinationArea.size.y / 2, destinationArea.transform.position.y + destinationArea.size.y / 2), destinationArea.transform.position.z);
        direction = (endPosition - transform.position).normalized;
    }

	void FixedUpdate ()
    {
        objectRigidBody.MovePosition(transform.position + direction * speed * Time.deltaTime);

    }
	
}
