using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour
{
    [SerializeField] [Range(0, 90f)] private float rotateSpeed = 10;
    [SerializeField] [Range(-0.1f, 0.1f)] private float moveSpeed = 0.05f;
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotateSpeed);

        if(transform.position.x > 7 && moveSpeed > 0)
        {
            moveSpeed = -moveSpeed;
        }
        else if(transform.position.x < -7 && moveSpeed < 0)
        {
            moveSpeed = -moveSpeed;
        }

        transform.position += new Vector3(moveSpeed, 0, 0);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().DropGrip();
        }
    }
}
