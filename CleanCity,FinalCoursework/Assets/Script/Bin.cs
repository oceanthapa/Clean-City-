using UnityEngine;

public class Bin : MonoBehaviour
{
    public int capacity = 15;
    private int currentCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Works even if collider is on child object
        Trash trash = other.GetComponentInParent<Trash>();

        if (trash != null)
        {
            if (currentCount < capacity)
            {
                currentCount++;

                // Destroy the whole trash object
                Destroy(trash.gameObject);

                Debug.Log("Trash Collected: " + currentCount + "/" + capacity);
            }
            else
            {
                Debug.Log("Bin is FULL!");
            }
        }
    }
}