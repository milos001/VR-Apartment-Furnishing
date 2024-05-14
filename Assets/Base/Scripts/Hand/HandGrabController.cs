using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandGrabController : MonoBehaviour
{
    [SerializeField]
    private Transform _grabTarget;
    [SerializeField]
    private InputActionProperty _grabAction, _velocityAction, _angularVelocityAction;

    private InputDevice _controller;

    private Object _objectInRange, _heldObject;

    private void Start()
    {
        _grabAction.action.started += TryPickUpObject;
        _grabAction.action.canceled += TryReleaseObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Object"))
            return;

        Object objectInRange = other.GetComponent<Object>();
        _objectInRange = objectInRange;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Object"))
            return;

        if (_objectInRange == other.GetComponent<Object>())
            _objectInRange = null;
    }

    private void TryPickUpObject(InputAction.CallbackContext context)
    {
        if (_objectInRange == null || !_objectInRange.CanPickUp())
            return;

        _objectInRange.PickUp(_grabTarget);
        _heldObject = _objectInRange;
    }

    private void TryReleaseObject(InputAction.CallbackContext context)
    {
        if (_heldObject == null)
            return;

        _heldObject.Release(_velocityAction.action.ReadValue<Vector3>(), _angularVelocityAction.action.ReadValue<Vector3>());
        _heldObject = null;
    }
}
