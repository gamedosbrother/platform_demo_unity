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
    private bool canAirJump;
    private bool canGlide;
    private float glideForce;
    [SerializeField]
    private float fallTimeMultiplier;
    private bool isGliding;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        distToGround = collider.bounds.extents.y;        
        
        float gravity = Physics.gravity.y;
        jumpForce = Mathf.Sqrt( jumpHeight * 2f * -gravity );
        glideForce = -gravity / fallTimeMultiplier;
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
            isGliding = true;
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            isGliding = false;
        }


    }

    void Jump() 
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = jumpForce;
        rigidbody.velocity = velocity;
    }

    void Glide() 
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = velocity.y + glideForce * Time.fixedDeltaTime;
        rigidbody.velocity = velocity;
    }

    void FixedUpdate()
    {
        bool isGrounded = IsGrounded();

        rigidbody.velocity = Vector3.up * rigidbody.velocity.y + verticalMovement * moveSpeed * transform.forward;
        rigidbody.MoveRotation(
            Quaternion.Euler(0f, transform.eulerAngles.y + (horizontalMovement * rotationSpeed * Time.fixedDeltaTime), 0f)
        );

        if (isGrounded) 
        {
            canAirJump = true;
            canGlide = false;
        }


        if (isGliding && rigidbody.velocity.y < 0f) { 

            Glide();
            Debug.Log("ta voando");

        }

        if (hasJumpedLastFrame)
        {
            hasJumpedLastFrame = false;

            if (isGrounded)
            {
                Jump();
            } else if (canAirJump)
            {
                Jump();
                canAirJump = false;
                canGlide = true;

            }
        }
        
    }

}
