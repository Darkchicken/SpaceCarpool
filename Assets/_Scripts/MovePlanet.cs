using UnityEngine;
using System.Collections;

public class MovePlanet : MonoBehaviour {

    public int speed;

    private Rigidbody objectRigidBody;
    private Vector3 direction;

    void Start()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        direction = (new Vector3(transform.position.x, transform.position.y, 0) - transform.position).normalized;
    }

    void FixedUpdate()
    {
            objectRigidBody.MovePosition(transform.position + direction * Time.deltaTime * speed);

    }
}
