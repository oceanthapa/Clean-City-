using UnityEngine;
using System.Collections;
using StarterAssets;

public class AITrashCollector : MonoBehaviour
{
    public float arrivalDistance = 1.5f;
    public Transform binTransform;

    private AIWalker movement;
    private Transform currentTarget;
    private bool hasTrash = false;
    private GameObject carriedObject;

    void Awake()
    {
        movement = GetComponent<AIWalker>();
    }

    void Start()
    {
        StartCoroutine(UpdateTarget());
    }

    IEnumerator UpdateTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (!hasTrash)
                currentTarget = GetNearestTrash();
            else
                currentTarget = binTransform;
        }
    }

    Transform GetNearestTrash()
    {
        Trash[] all = FindObjectsOfType<Trash>();
        Transform nearest = null;
        float closest = Mathf.Infinity;

        foreach (Trash t in all)
        {
            if (t.transform.parent != null) continue;
            float d = Vector3.Distance(transform.position, t.transform.position);
            if (d < closest)
            {
                closest = d;
                nearest = t.transform;
            }
        }
        return nearest;
    }

    void Update()
    {
        if (movement == null) return;

        if (currentTarget == null)
        {
            movement.moveDirection = Vector3.zero;
            return;
        }

        Vector3 toTarget = currentTarget.position - transform.position;
        toTarget.y = 0f;
        float dist = toTarget.magnitude;

        if (dist > arrivalDistance)
        {
            movement.moveDirection = toTarget.normalized;
        }
        else
        {
            movement.moveDirection = Vector3.zero;
            if (!hasTrash)
                PickUp();
            else
                Drop();
        }
    }

    void PickUp()
    {
        if (currentTarget == null) return;
        Trash t = currentTarget.GetComponent<Trash>();
        if (t == null) return;

        carriedObject = t.gameObject;
        carriedObject.transform.SetParent(transform);
        carriedObject.transform.localPosition = new Vector3(0, 2f, 0);

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = carriedObject.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        hasTrash = true;
        currentTarget = binTransform;
        movement.moveDirection = (binTransform.position - transform.position).normalized;
    }

    void Drop()
    {
        if (carriedObject != null)
        {
            Destroy(carriedObject);
            carriedObject = null;

            if (GameManager.Instance != null)
            {
                Bin bin = binTransform.GetComponent<Bin>();
                if (bin != null)
                    GameManager.Instance.AddScore(bin.playerIndex);
            }
        }

        hasTrash = false;
        currentTarget = GetNearestTrash();
    }
}