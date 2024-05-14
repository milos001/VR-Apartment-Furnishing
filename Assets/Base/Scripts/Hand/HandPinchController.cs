using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandPinchController : MonoBehaviour
{
    [SerializeField]
    private HandType _handType;
    [SerializeField]
    private GazeSelectionController _selectionController;
    [SerializeField]
    private InputActionProperty _pinchValueAction, _handPosAction;

    private Object _heldObject;

    private Vector3 _lastPinchPosition;
    private float _moveSpeed = 3000f, _rotateSpeed = 100000f;
    private bool _pinching;

    // Start is called before the first frame update
    private void Start()
    {
        _pinchValueAction.action.started += x =>
        {
            _lastPinchPosition = _handPosAction.action.ReadValue<Vector3>();
            _pinching = true;
        };
        _pinchValueAction.action.canceled += x => _pinching = false;

        _selectionController.OnSelectionChange += SelectionChangedHandler;
    }

    private void OnDestroy()
    {
        _selectionController.OnSelectionChange -= SelectionChangedHandler;
    }

    private void Update()
    {
        if (!_pinching || _heldObject == null)
            return;

        switch (_handType)
        {
            case HandType.Left:
                _heldObject.RotateAmountInDirection(_handPosAction.action.ReadValue<Vector3>() - _lastPinchPosition, _pinchValueAction.action.ReadValue<float>() * Time.deltaTime * _rotateSpeed);
                break;
            case HandType.Right:
                _heldObject.MoveAmountInDirection(_handPosAction.action.ReadValue<Vector3>() - _lastPinchPosition, _pinchValueAction.action.ReadValue<float>() * Time.deltaTime * _moveSpeed);
                break;
        }

        _lastPinchPosition = _handPosAction.action.ReadValue<Vector3>();
    }

    private void SelectionChangedHandler(Object selection)
    {
        _heldObject = selection;
    }
}

enum HandType
{
    Left,
    Right
}
