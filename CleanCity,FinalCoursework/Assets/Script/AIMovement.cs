using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float speed = 4f;
    public float rotationSpeed = 10f;
    public float gravity = -20f;

    [HideInInspector] public Vector3 moveDirection = Vector3.zero;

    private CharacterController controller;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Gravity
        if (controller.isGrounded)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        // Move
        Vector3 motion = moveDirection * speed;
        motion.y = verticalVelocity;
        controller.Move(motion * Time.deltaTime);

        // Rotate to face direction
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion target = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, target, rotationSpeed * Time.deltaTime);
        }
    }
}