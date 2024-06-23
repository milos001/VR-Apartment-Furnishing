using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteractionController : MonoBehaviour
{
    [SerializeField]
    private XRRayInteractor _rayInteractor;

    private Vector3 _UIWorldPosition;
    private bool _isOnUI;

    // Update is called once per frame
    void Update()
    {
        _rayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult hit);
        if(hit.isValid)
            _UIWorldPosition = hit.worldPosition;
        _isOnUI = hit.isValid;
    }

    public Vector3 GetUIWorldPosition()
    {
        return _UIWorldPosition;
    }

    public bool IsOnUI()
    {
        return _isOnUI;
    }
}
