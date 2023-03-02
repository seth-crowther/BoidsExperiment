using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Camera playerCam;
    private float lookSpeed = 50f;
    private float rotX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float look_x = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
        float look_y = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

        rotX += look_y;
        rotX = Mathf.Clamp(rotX, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(-rotX, 0f, 0f);
        transform.Rotate(new Vector3(0, look_x, 0));
    }
}
