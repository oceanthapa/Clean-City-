using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRadius = 3f;
    private Trash heldTrash;
    private Rigidbody heldRb;
    private Collider heldCol;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldTrash == null)
                TryPickup();
            else
                DropTrash();
        }

        if (heldTrash != null)
        {
            heldTrash.transform.position =
                transform.position + transform.forward * 1.5f + Vector3.up * 1.5f;
        }
    }

    void TryPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRadius);

        foreach (Collider hit in hits)
        {
            // Skip self and children
            if (hit.transform.IsChildOf(transform)) continue;

            Trash trash = hit.GetComponentInParent<Trash>();
            if (trash == null) continue;

            // Skip rocks and anything without Rigidbody
            Rigidbody rb = trash.GetComponent<Rigidbody>();
            if (rb == null) continue;

            heldTrash = trash;
            heldRb = rb;

            heldRb.isKinematic = true;
            heldRb.useGravity = false;

            // Disable collider so it cant be picked up again
            heldCol = trash.GetComponent<Collider>();
            if (heldCol == null)
                heldCol = trash.GetComponentInChildren<Collider>();
            if (heldCol != null)
                heldCol.enabled = false;

            Debug.Log("Picked up: " + trash.name);
            return;
        }

        Debug.Log("No trash nearby");
    }

    void DropTrash()
    {
        if (heldRb != null)
        {
            heldRb.isKinematic = false;
            heldRb.useGravity = true;
        }

        if (heldCol != null)
            heldCol.enabled = true;

        heldTrash = null;
        heldRb = null;
        heldCol = null;
    }
}