using UnityEngine;

public interface IPickable
{
    void PickUp(Transform parent);
    void Throw(Vector3 direction, float force);
}

