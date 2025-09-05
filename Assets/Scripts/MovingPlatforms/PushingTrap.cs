using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingTrap : MonoBehaviour
{

    [SerializeField] private PushingObstacle _pushingObstacle;

    private Vector3 _startPos;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponentInParent<Rigidbody>();
           
            if (playerRb != null)
            {
                if (_pushingObstacle._isPushing)
                {
                    Debug.Log($"Pushing trap activated on player: {collision.gameObject.name}");
                    Vector3 pushDirection = collision.transform.position - transform.position;
                    pushDirection.y = 0; // Keep the push horizontal
                    playerRb.AddForce(pushDirection.normalized * _pushingObstacle._strength, ForceMode.Impulse);
                    Debug.Log($"Player {collision.gameObject.name} pushed by trap.");
                }
            }
            else
            {
                Debug.LogError("Rigidbody component is missing on the player object.");
            }
        }
    }

    private void Update()
    {
        Debug.Log($"Is Pushing: {_pushingObstacle._isPushing}, Is Moving: {_pushingObstacle._isMoving}");
    }

    public void Awake()
    {
        //_startPos = transform.position;
    }

    public void Start()
    {
        //transform.position = _startPos;
    }

    private void OnEnable()
    {
        //transform.position = _startPos;
    }
}
