using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [Header("Looking around")]
    [SerializeField] Camera cam;
    [SerializeField] public float sensitivityX = 1.0f;
    [SerializeField] public float sensitivityY = 1.0f;

    [SerializeField][Range(1, 4)] float xRecoilCamPart;
    [SerializeField][Range(1, 4)] float yRecoilCamPart;

    public bool active { private get; set; } = true;

    public float xRotation { get; private set; }

    public float extraXRotation { get; set; }
    public float extraYRotation { get; set; }

    [SerializeField] Transform infrontOfCamHolder;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Rotate the camera up and down with a max of 90° up / down and side to side based on recoil
        //Rotate the player on the y axis based on simple mouse input

        float mouseY = active ? Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY * 1000 : 0;
        float mouseX = active ? Input.GetAxis("Mouse X") * Time.deltaTime * sensitivityX * 1000 : 0;

        MoveCam(mouseX, mouseY);
        
    }

    public void MoveCam(float _mouseX, float _mouseY)
    {
        xRotation -= _mouseY;
        xRotation = Mathf.Clamp(xRotation + (extraXRotation / xRecoilCamPart), -90f, 90f) - (extraXRotation / xRecoilCamPart);

        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, _mouseX + transform.eulerAngles.y, 0));

        cam.transform.localRotation = Quaternion.Euler(new Vector3(xRotation + (extraXRotation / xRecoilCamPart), (extraYRotation / yRecoilCamPart), 0));

        //Apply the rotation also to the infrontofcamholder which is used to determine recoil and shots
        infrontOfCamHolder.localRotation = Quaternion.Euler(new Vector3(xRotation + extraXRotation, extraYRotation, 0));
    }
}
