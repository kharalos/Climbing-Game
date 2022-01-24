using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public enum HandType { right, left }
    public HandType hand;

    [SerializeField] private Transform target;
    public Transform Target => target;


    private Rigidbody h_Rigidbody;

    [SerializeField] private bool active;

    [SerializeField] private float speed = 500;

    private bool justLaunched = false;
    private float timer = 0;

    public bool onGrip = false;

    private void Start()
    {
        h_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (justLaunched)
        {
            timer += Time.deltaTime;
            if(timer > 1.2f)
                justLaunched = false;
        }
        else
        {
            timer = 0;
        }
        if (active)
        {
            if ((target.position - transform.position).magnitude > 0.3f)
            {
                h_Rigidbody.AddForce((target.position - transform.position).normalized * speed, ForceMode.Force);

                if (!justLaunched) active = false;

                h_Rigidbody.isKinematic = false;

                onGrip = false;
            }
            else
            {
                h_Rigidbody.position = target.position + new Vector3(0, -0.2f, -0.1f);
                h_Rigidbody.isKinematic = true;

                if (hand == HandType.right)
                    transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0.5f, 0.5f, 0.5f, 0.5f), 0.1f);
                else
                    transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(-0.5f, 0.5f, -0.5f, 0.5f), 0.1f);

                onGrip = true;
            }
        }
        else { onGrip = false; h_Rigidbody.isKinematic = false; }
    }
    public void Activate()
    {
        active = true;
        justLaunched = true;
        timer = 0;
    }
    public void Disable()
    {
        active = false;
        justLaunched = false;
        timer = 0;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
