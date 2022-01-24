using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    [SerializeField] [Range(0,1)] private float lavaSpeed = 0.5f;
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * lavaSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().Defeat();
        }
    }
}
