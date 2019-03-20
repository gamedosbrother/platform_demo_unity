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
    private float jumpHeight;
    private float jumpForce;

    private Rigidbody rigidbody;
    private BoxCollider collider;

    private float verticalMovement;
    private float horizontalMovement;
    private float distToGround;
    private bool hasJumpedLastFrame;
    private bool hasJumped;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        distToGround = collider.bounds.extents.y;        
        
        float gravity = Physics.gravity.y;
        jumpForce = Mathf.Sqrt( jumpHeight * 2f * -gravity );
    }

    bool IsGrounded() 
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    void Update()
    {   
        verticalMovement = Input.GetAxisRaw("Vertical");
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            hasJumpedLastFrame = true;
        }

    }

    void Jump() 
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = jumpForce;
        rigidbody.velocity = velocity;
    }

    void FixedUpdate()
    {
        bool isGrounded = IsGrounded();

        rigidbody.velocity = Vector3.up * rigidbody.velocity.y + verticalMovement * moveSpeed * transform.forward;
        rigidbody.MoveRotation(
            Quaternion.Euler(0f, transform.eulerAngles.y + (horizontalMovement * rotationSpeed * Time.fixedDeltaTime), 0f)
        );

        if (hasJumpedLastFrame)
        {
            hasJumpedLastFrame = false;
            if (isGrounded)
            {
                hasJumped = true;
                Jump();
            } else if (hasJumped) 
            {
                Jump();
                hasJumped = false;
            }
        }
        
        

    }

}
