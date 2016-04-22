using UnityEngine;
using System.Collections;

public class MoveObjects : MonoBehaviour {

    public float speed = 1;
    public bool masterUpdated = false;
    public Vector3 endPosition;
    private BoxCollider destinationArea;
    private Rigidbody objectRigidBody;
    private float startTime;
    private Vector3 colliderSize;
    
    private Vector3 direction;
    private PhotonView photonView;
    

    void Awake()
    {
        startTime = Time.deltaTime;
        objectRigidBody = GetComponent<Rigidbody>();
        destinationArea = GameManager.gameManager.destinationArea;
        colliderSize = destinationArea.GetComponent<BoxCollider>().size;
        photonView = GetComponent<PhotonView>();
    }

    void OnEnable()
    {
        masterUpdated = false;
        
        if(PhotonNetwork.isMasterClient)
        {
            endPosition = new Vector3(Random.Range(destinationArea.transform.position.x - destinationArea.size.x / 2, destinationArea.transform.position.x + destinationArea.size.x / 2),
            Random.Range(destinationArea.transform.position.y - destinationArea.size.y / 2, destinationArea.transform.position.y + destinationArea.size.y / 2), destinationArea.transform.position.z);
        }
        
    }

    public void ResourceLoad()
    {
        OnEnable();
    }


    [PunRPC]
    void SetDirection(Vector3 endPosition)
    {
        direction = (endPosition - transform.position).normalized;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    void FixedUpdate ()
    {
        if(masterUpdated)
        {
            objectRigidBody.MovePosition(transform.position + direction * speed * Time.deltaTime);

        }
    }
    public void SetResourceDirection(Vector3 _direction)
    {
        direction = _direction;
    }

    
	
}
