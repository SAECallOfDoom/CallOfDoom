using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    public GameObject testPlane;  

    /// <summary>
    /// Private reference to player.-Transform
    /// </summary>
    [SerializeField]
    private Transform playerBody;
    
    /// <summary>
    /// Private mouse sensitivity value.-Float
    /// </summary>
    [SerializeField]
    private float mouseSensitivity = 100.0f;

    /// <summary>
    /// Private. Stores mouse x dimension rotation. -Float
    /// </summary>
    private float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
