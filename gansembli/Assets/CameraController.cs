using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float minFov = 40f;

    [SerializeField]
    private float maxFov = 90f;

    [SerializeField]
    private float sensitivity = 3f;

    private float fov;

    // Use this for initialization
    private void Start()
    {
        fov = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    private void Update()
    {
        // Zooming in
        if (Input.GetKey(KeyCode.Mouse1))
        {
            fov -= sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
        }
        else if (fov != maxFov)
        {
            fov += sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
        }

        Camera.main.fieldOfView = fov;
    }
}