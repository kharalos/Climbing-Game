using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float camY = 2f, camZ = -5f;
    [SerializeField] [Range(0,1f)] private float camLerpTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTarget);
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position + new Vector3(0, camY, camZ), camLerpTime);
    }

    public GameObject GetClickedObject()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Rock"))
            {
                Debug.Log("Touch on Rock.");
                return hit.collider.gameObject;
            }
            else return null;
        }
        else return null;
    }
}
