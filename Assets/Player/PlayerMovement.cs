using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float jumpForce;

    private Rigidbody rigidbody;
    private BoxCollider collider;

    private float verticalMovement;
    private float horizontalMovement;
    private float distToGround;
    private bool jumped;
    private bool firstJump;
    private bool secondJump;
    


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        distToGround = collider.bounds.extents.y;

    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void Update()
    {   
        verticalMovement = Input.GetAxisRaw("Vertical");
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown("space")) {
            jumped = true;
        }

    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0) + verticalMovement * moveSpeed * transform.forward;
        rigidbody.MoveRotation(
            Quaternion.Euler(0f, transform.eulerAngles.y + (horizontalMovement * rotationSpeed * Time.fixedDeltaTime), 0f)
        );
        
        if (jumped) {
            jumped = false;
            Debug.Log("Pulou");
            
            if (!secondJump && firstJump){
                Debug.Log("Pulo2");
                rigidbody.AddForce(0f, jumpForce * Time.fixedDeltaTime, 0f);
                secondJump = true;
            } else if (!firstJump) {
                Debug.Log("Pulo1");
                rigidbody.AddForce(0f, jumpForce * Time.fixedDeltaTime, 0f);
                firstJump = true;
            }
          
        }
        
        if (IsGrounded()) {
            Debug.Log("Resetou");
            firstJump = false;
            secondJump = false;
            jumped = false;
        }
    }

}
