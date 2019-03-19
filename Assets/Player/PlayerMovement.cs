using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationSpeed;

    private Rigidbody rigidbody;

    private float verticalMovement;
    private float horizontalMovement;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {   
        verticalMovement = Input.GetAxisRaw("Vertical");
        horizontalMovement = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        rigidbody.velocity = transform.forward * verticalMovement * moveSpeed;
        rigidbody.MoveRotation(
            Quaternion.Euler(0f, transform.eulerAngles.y + (horizontalMovement * rotationSpeed * Time.fixedDeltaTime), 0f)
        );
    }

}
