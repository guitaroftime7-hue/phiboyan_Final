using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Transform cameraRoot;
    public float walkSpeed = 3.5f;
    public float runSpeed = 6f;
    public float gravity = -20f;

    public float mouseSensitivity = 2f;
    public float maxLookUp = 80f;

    CharacterController cc;
    Vector3 vel;
    float xRot;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        Move();
    }

    void Look()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mx);

        xRot -= my;
        xRot = Mathf.Clamp(xRot, -maxLookUp, maxLookUp);
        cameraRoot.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    void Move()
    {
        if (cc.isGrounded && vel.y < 0) vel.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        cc.Move(move * speed * Time.deltaTime);

        vel.y += gravity * Time.deltaTime;
        cc.Move(vel * Time.deltaTime);
    }
}
