using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    private Transform player;
    private Vector3 offset;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>().transform;
        offset = player.position - transform.position;
    }

    void LateUpdate()
    {
        // transform.LookAt(player);
        transform.position = player.position - offset;
    }
}
