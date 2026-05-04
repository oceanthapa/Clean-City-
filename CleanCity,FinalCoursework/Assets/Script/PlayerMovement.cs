using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public string horizontalInput;
    public string verticalInput;

    void Update()
    {
        float h = Input.GetAxis(horizontalInput);
        float v = Input.GetAxis(verticalInput);

        Vector3 move = new Vector3(h, 0, v);
        transform.Translate(move * speed * Time.deltaTime);
    }
}