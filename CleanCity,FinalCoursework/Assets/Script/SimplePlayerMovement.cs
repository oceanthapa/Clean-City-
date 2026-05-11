using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 1.2f;
    public float gravity = -20f;
    public Camera playerCamera;

    private CharacterController controller;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * v + camRight * h;

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;
        moveDir.y = verticalVelocity;

        controller.Move(moveDir * speed * Time.deltaTime);

        Vector3 flat = new Vector3(moveDir.x, 0, moveDir.z);
        if (flat.magnitude > 0.1f)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(flat),
                10f * Time.deltaTime);
    }
}