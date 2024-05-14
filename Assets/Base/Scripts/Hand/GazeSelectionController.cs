using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GazeSelectionController : MonoBehaviour
{
    public Action<Object> OnSelectionChange;

    [SerializeField]
    private InputActionProperty _rightPrimaryActionProperty, _rightSecondaryActionProperty;
    [SerializeField]
    private Material _selectionMat;

    private Object _selectedObject;

    private bool _selectionEnabled;

    // Start is called before the first frame update
    void Start()
    {
        _rightPrimaryActionProperty.action.performed += EnableDisableSelection;
        _rightSecondaryActionProperty.action.performed += TryDeleteObject;
    }

    private void EnableDisableSelection(InputAction.CallbackContext obj)
    {
        if (_selectionEnabled && _selectedObject != null)
        {
            _selectedObject.SetSelected(false, _selectionMat);
            _selectedObject = null;
            OnSelectionChange.Invoke(null);
            _selectionEnabled = true;
        }
        else if(!_selectionEnabled)
            TrySelectObject();

        _selectionEnabled = !_selectionEnabled;
    }

    // Update is called once per frame
    private bool TrySelectObject()
    {
        if (!Physics.SphereCast(transform.position, 1f, transform.forward, out RaycastHit hit, 1000f, 1 << LayerMask.NameToLayer("Object")))
            return false;

        Object selectedObject = hit.collider.GetComponent<Object>();
        if (selectedObject != null && selectedObject != _selectedObject)
        {
            if (_selectedObject != null)
                _selectedObject.SetSelected(false, _selectionMat);

            selectedObject.SetSelected(true, _selectionMat);
            _selectedObject = selectedObject;
            OnSelectionChange.Invoke(selectedObject);
            return true;
        }
        else
            return false;
    }

    private void TryDeleteObject(InputAction.CallbackContext obj)
    {
        if (_selectedObject == null)
            return;

        Destroy(_selectedObject.gameObject);
        _selectedObject = null;
        _selectionEnabled = false;
    }
}
