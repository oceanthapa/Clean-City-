using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Camera playerCamera;
    public float pickupRange = 10f;
    public float holdDistance = 2f;

    private Trash heldTrash;
    private Rigidbody heldRb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldTrash == null)
            {
                TryPickup();
            }
            else
            {
                DropTrash();
            }
        }

        // Hold object in front of camera
        if (heldTrash != null)
        {
            heldTrash.transform.position =
                playerCamera.transform.position +
                playerCamera.transform.forward * holdDistance;
        }
    }

    void TryPickup()
    {
        RaycastHit hit;

        Vector3 rayOrigin =
            playerCamera.transform.position + playerCamera.transform.forward * 0.5f;

        Vector3 rayDirection =
            (playerCamera.transform.forward + Vector3.down * 0.2f).normalized;

        Debug.DrawRay(rayOrigin, rayDirection * pickupRange, Color.red, 3f);

        // 🔥 IMPORTANT: ignore Player layer completely
        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, pickupRange, layerMask))
        {
            Debug.Log("Hit object: " + hit.collider.name);

            Trash trash = hit.collider.GetComponentInParent<Trash>();

            if (trash != null)
            {
                Debug.Log("Trash found!");

                heldTrash = trash;
                heldRb = trash.GetComponent<Rigidbody>();

                if (heldRb != null)
                {
                    heldRb.isKinematic = true;
                    heldRb.useGravity = false;
                }
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing");
        }
    }

    void DropTrash()
    {
        if (heldRb != null)
        {
            heldRb.isKinematic = false;
            heldRb.useGravity = true;
        }

        heldTrash = null;
        heldRb = null;
    }
}