using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private AudioClip deathSound;

    private AudioSource audioSource;

    private Transform player;

    private float health;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindObjectOfType<PlayerMovement>().transform;
        health = maxHealth;
    }

    void Update()
    {
        if(health <= 0f) return;
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
    
    public void DealDamage(float damage)
    {
        if(health <= 0f) return;

        health -= damage;
        if(health <= 0f)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
        }
        else
        {
            audioSource.Play();
        }
    }

}
