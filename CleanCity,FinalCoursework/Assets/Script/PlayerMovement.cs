using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Input (leave blank for AI control)")]
    public string horizontalInput;   // e.g. "Horizontal", "Horizontal2"
    public string verticalInput;     // e.g. "Vertical",   "Vertical2"
    public bool isHumanControlled = false;

    [Header("References")]
    public Camera playerCamera;

    // AI uses this to feed movement direction
    [HideInInspector] public Vector3 aiMoveDirection = Vector3.zero;

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;

        if (isHumanControlled)
        {
            float h = Input.GetAxis(horizontalInput);
            float v = Input.GetAxis(verticalInput);

            // Move relative to camera direction
            Vector3 camForward = playerCamera.transform.forward;
            Vector3 camRight   = playerCamera.transform.right;
            camForward.y = 0f;
            camRight.y   = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDir = (camForward * v + camRight * h);
        }
        else
        {
            moveDir = aiMoveDirection;
        }

        // Gravity
        moveDir.y = -9.8f * Time.deltaTime;

        if (controller != null)
            controller.Move(moveDir * speed * Time.deltaTime);

        // Rotate character to face movement direction
        Vector3 flatDir = new Vector3(moveDir.x, 0, moveDir.z);
        if (flatDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}