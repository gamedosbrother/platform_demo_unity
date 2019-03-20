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
    [SerializeField]
    private float fallTimeMultiplier;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashDeaccelerationTime;

    private Rigidbody rigidbody;
    private CapsuleCollider collider;

    private float verticalMovement;
    private float horizontalMovement;

    private float distanceToGround;

    private float jumpForce;
    private bool hasJumpedLastFrame;
    private bool canAirJump;

    private float glideForce;
    private bool isGliding;

    private Vector3 dashVelocity;
    private float dashDeacceleration;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        distanceToGround = collider.bounds.extents.y;        
        
        float gravity = Physics.gravity.y;
        jumpForce = Mathf.Sqrt( jumpHeight * 2f * -gravity );
        glideForce = -gravity / fallTimeMultiplier;
        dashDeacceleration = dashSpeed / dashDeaccelerationTime;
    }

    bool IsGrounded() 
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);
    }

    void Update()
    {   
        verticalMovement = Input.GetAxisRaw("Vertical");
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasJumpedLastFrame = true;
            isGliding = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isGliding = false;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashVelocity = transform.forward * dashSpeed;
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

        Vector3 moveVelocity = transform.forward * verticalMovement * moveSpeed;

        rigidbody.velocity = (Vector3.up * rigidbody.velocity.y) + moveVelocity + dashVelocity;
        rigidbody.MoveRotation(
            Quaternion.Euler(0f, transform.eulerAngles.y + (horizontalMovement * rotationSpeed * Time.fixedDeltaTime), 0f)
        );

        dashVelocity = Vector3.MoveTowards(dashVelocity, Vector3.zero, dashDeacceleration * Time.fixedDeltaTime);

        if (isGrounded) 
        {
            canAirJump = true;
        }


        if (isGliding && rigidbody.velocity.y < 0f) { 

            Glide();

        }

        if (hasJumpedLastFrame)
        {
            hasJumpedLastFrame = false;

            if (isGrounded)
            {
                Jump();
            }
            else if (canAirJump)
            {
                Jump();
                canAirJump = false;
            }
        }
        
    }

}
