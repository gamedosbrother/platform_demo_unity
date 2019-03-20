using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField]
    private float attackDelay;

    private Camera camera;
    private AudioSource audioSource;

    private bool isAttacking;

    void Awake()
    {
        camera = Camera.main;    
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(AttackCycle());
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAttacking = true;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            isAttacking = false;
        }

    }

    IEnumerator AttackCycle()
    {
        while (true)
        {
            if(isAttacking) Attack();
            yield return new WaitForSeconds(attackDelay);
        }
    }

    void Attack()
    {
        audioSource.Play();
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red, attackDelay * .9f);

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.DealDamage(10f);
            }
        }
    }

}
