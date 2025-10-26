using UnityEngine;

public class Pickable : MonoBehaviour, IPickable
{
    public float throwForce = 10f;

    // ----------------------------
    // Pick Up
    // ----------------------------
    public void PickUp(Transform parent)
    {
        // Parent to the slot
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Remove Rigidbody if exists
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            Destroy(rb);
    }

    // ----------------------------
    // Throw
    // ----------------------------
    public void Throw(Vector3 direction, float forceMultiplier = 1f)
    {
        // Detach from parent
        transform.SetParent(null);

        // Add Rigidbody for physics
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero; // reset any previous velocity

        // Apply force
        rb.AddForce(direction.normalized * throwForce * forceMultiplier, ForceMode.VelocityChange);
    }
}
