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
    [SerializeField]
    private float wallJumpSpeed;
    [SerializeField]
    private float wallJumpDeacceleration;
    [SerializeField]
    private float wallJumpForce;

    private Rigidbody rigidbody;
    private BoxCollider collider;

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

    private float distanceToWall;
    private bool canWallJump;
    private Vector3 wallJumpVelocity;

    void Awake ()
    {
        rigidbody = GetComponent<Rigidbody> ();
        collider = GetComponent<BoxCollider> ();
        distanceToGround = collider.bounds.extents.y;
        distanceToWall = collider.bounds.extents.z;

        float gravity = Physics.gravity.y;
        jumpForce = Mathf.Sqrt (jumpHeight * 2f * -gravity);
        glideForce = -gravity / fallTimeMultiplier;
        dashDeacceleration = dashSpeed / dashDeaccelerationTime;
    }

    bool IsGrounded ()
    {
        return Physics.Raycast (transform.position, Vector3.down, distanceToGround + 0.1f);
    }

    bool IsWalled ()
    {
        return Physics.Raycast (transform.position, transform.forward, distanceToWall + 0.1f);
    }

    void Update ()
    {

        verticalMovement = Input.GetAxisRaw ("Vertical");
        horizontalMovement = Input.GetAxisRaw ("Horizontal");

        if (Input.GetKeyDown (KeyCode.Space))
        {
            hasJumpedLastFrame = true;
            isGliding = true;
        }
        else if (Input.GetKeyUp (KeyCode.Space))
        {
            isGliding = false;
        }

        if (Input.GetKeyDown (KeyCode.LeftShift))
        {
            dashVelocity = transform.forward * dashSpeed;
        }

    }

    void Jump ()
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = jumpForce;
        rigidbody.velocity = velocity;
    }

    void Glide ()
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = velocity.y + glideForce * Time.fixedDeltaTime;
        rigidbody.velocity = velocity;
    }

    void WallJump ()
    {
        wallJumpVelocity = -transform.forward * wallJumpSpeed;
        Vector3 velocity = rigidbody.velocity;
        rigidbody.velocity = (Vector3.up * wallJumpForce) + wallJumpVelocity;
        StartCoroutine (PlayerRotation ());
    }

    IEnumerator PlayerRotation ()
    {
        for (int i = 0; i < 6; i++)
        {
            transform.rotation = Quaternion.Euler (0f, transform.eulerAngles.y + 30f, 0f);

            yield return new WaitForSeconds (0.1f);
        }
    }

    void FixedUpdate ()
    {
        bool isGrounded = IsGrounded ();
        bool isWalled = IsWalled ();

        Vector3 moveVelocity = transform.forward * verticalMovement * moveSpeed;

        rigidbody.velocity = (Vector3.up * rigidbody.velocity.y) + moveVelocity + dashVelocity + wallJumpVelocity;
        rigidbody.MoveRotation (
            Quaternion.Euler (0f, transform.eulerAngles.y + (horizontalMovement * rotationSpeed * Time.fixedDeltaTime), 0f)
        );

        wallJumpVelocity = Vector3.MoveTowards (wallJumpVelocity, Vector3.zero, wallJumpDeacceleration * Time.fixedDeltaTime);
        dashVelocity = Vector3.MoveTowards (dashVelocity, Vector3.zero, dashDeacceleration * Time.fixedDeltaTime);

        if (isGrounded)
        {
            canAirJump = true;
        }

        if (isGliding && rigidbody.velocity.y < 0f)
        {
            Glide ();
        }

        if (isWalled && !isGrounded)
        {
            canWallJump = true;
        }

        if (hasJumpedLastFrame)
        {
            hasJumpedLastFrame = false;

            if (isWalled && !isGrounded)
            {
                WallJump ();
                Debug.Log ("walljump0");
            }
            else if (isGrounded)
            {
                Jump ();
            }
            else if (canAirJump)
            {
                Jump ();
                canAirJump = false;
            }

        }

    }

}