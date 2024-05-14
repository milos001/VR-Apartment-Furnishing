using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Object : Entity
{
    [SerializeField]
    private bool _canBePickedUp;

    private void Start()
    {
        if(rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
        if(meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
    }

    public bool CanPickUp()
    {
        return _canBePickedUp;
    }

    public void PickUp(Transform grabParent)
    {
        transform.parent = grabParent;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.useGravity = false;
    }

    public void Release(Vector3 velocity, Vector3 angularVelocity)
    {
        transform.parent = null;

        rigidbody.useGravity = true;
        rigidbody.velocity = velocity;
        rigidbody.angularVelocity = angularVelocity;
    }

    public void SetSelected(bool selected, Material selectionMat)
    {
            List<Material> mats = meshRenderer.materials.ToList();
        if (selected)
        {
            mats.Add(selectionMat);
        }
        else
        {
            mats.RemoveAt(mats.Count - 1);
        }

        meshRenderer.materials = mats.ToArray();
        rigidbody.useGravity = !selected;
    }
}
