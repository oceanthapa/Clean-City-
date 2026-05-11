using UnityEngine;

public class FinalPlayerMove : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController cc;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, -9.8f * Time.deltaTime, v);
        cc.Move(move * speed * Time.deltaTime);
    }
}