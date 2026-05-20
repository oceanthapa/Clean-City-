using UnityEngine;

public class Bin : MonoBehaviour
{
    public int playerIndex = 0;
    public int capacity = 15;
    private int currentCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        Trash trash = other.GetComponentInParent<Trash>();
        if (trash != null && currentCount < capacity)
        {
            currentCount++;
            Destroy(trash.gameObject);
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(playerIndex);
        }
    }
}