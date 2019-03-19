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
        // Show!
        // Só não entendi pq não está pegando o Player haha, mas depois vemos isso.
        // Aqui vou explicar sobre Layers pra vcs
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    void Update()
    {   
        verticalMovement = Input.GetAxisRaw("Vertical");
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        // Evitem usar string para os GetKey, usem os KeyCode
        if (Input.GetKeyDown(KeyCode.Space)) {
            jumped = true;
        }

    }

    void FixedUpdate()
    {
        // Certinho o que tinha que ser feito, mas, sugiro trocar por Vector3.down * rigidbody.velocity.y
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0) + verticalMovement * moveSpeed * transform.forward;
        rigidbody.MoveRotation(
            Quaternion.Euler(0f, transform.eulerAngles.y + (horizontalMovement * rotationSpeed * Time.fixedDeltaTime), 0f)
        );
        
        if (jumped) {
            jumped = false;
            Debug.Log("Pulou");
            
            if (!secondJump && firstJump){
                Debug.Log("Pulo2");
                // Não precisa do Time.fixedDeltaTime aqui. O AddForce adiciona a força e deixa pra física controlar, já que ele não vai alterar todo frame
                // Só precisa adicionar o valor.
                //
                // EU ACHO que fica mais legível se, em vez de mandar 3 parametros, mandar só 1 com Vector3.up * jumpForce
                //
                // Centralizar isso para uma função, não precisamos chamar 2 vezes a mesma coisa
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
