using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    private Camera camera;

    void Awake()
    {
        camera = Camera.main;    
    }

    void Update()
    {

        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(mouseRay, out hit))
        {
            Vector3 position = transform.position;
            position.y = hit.point.y;

            Vector3 direction = Vector3.Normalize(hit.point - position);
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0f, angle, 0f);
        }

    }
}
