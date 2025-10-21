using UnityEngine;

public class Pickable : MonoBehaviour
{
    public void HandlePickUp()
    {
        Transform playerSlot = Player.Instance.playerPickedUpObjectSlot;

        if (playerSlot.childCount == 1)
        {
            Transform previousObject = playerSlot.GetChild(0);
            ExecuteThrow(previousObject);
        }
        ExecutePickUp(transform, playerSlot);

    }

    private void ExecutePickUp(Transform objectToPickUp, Transform pickedUpObjectSlot)
    {

        Rigidbody objectToPickUpRigidbody = objectToPickUp.GetComponent<Rigidbody>();
        if (objectToPickUpRigidbody != null)
        {
            Destroy(objectToPickUpRigidbody);
        }
        SetParentAndLayer(objectToPickUp, pickedUpObjectSlot, "PickedUp");
        objectToPickUp.localPosition = Vector3.zero;
        objectToPickUp.localRotation = Quaternion.identity;
    }

    public void ExecuteThrow(Transform objectToThrow)
    {
        SetParentAndLayer(objectToThrow, null, "Default");
        objectToThrow.gameObject.AddComponent<Rigidbody>();
    }
    private void SetParentAndLayer(Transform target, Transform newParent, string layerName)
    {
        target.SetParent(newParent);
        target.gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
