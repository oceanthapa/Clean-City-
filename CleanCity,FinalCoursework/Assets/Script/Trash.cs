using UnityEngine;

public class Trash : MonoBehaviour
{
    public void PickUp(Transform holdPoint)
    {
        // Disable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Move to player hand
        transform.position = holdPoint.position;

        // Stick to player
        transform.SetParent(holdPoint);
    }

    public void Drop()
    {
        // Enable physics again
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        transform.SetParent(null);
    }
}