using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandPinchController : MonoBehaviour
{
    [SerializeField]
    private HandType _handType;
    [SerializeField]
    private HandPinchController _counterpartController;
    [SerializeField]
    private GazeSelectionController _selectionController;
    [SerializeField]
    private HoveringUIManager _uiManager;
    [SerializeField]
    private InputActionProperty _pinchValueAction, _handPosAction, _leftSecondaryAction;

    private Object _heldObject;

    private Vector3 _lastPinchPosition, _startingPosition, _startingObjectValue;
    private float _moveSpeed = 3000f, _rotateSpeed = 100000f, _scaleSpeed = 200f;
    private float _lastDistance;
    private bool _pinching;
    private int _snapMode, _largestIndex;

    // Start is called before the first frame update
    private void Start()
    {
        _pinchValueAction.action.started += x =>
        {
            _lastPinchPosition = _handPosAction.action.ReadValue<Vector3>();
            _startingPosition = _handPosAction.action.ReadValue<Vector3>();
            if(_heldObject != null)
            {
                switch (_handType)
                {
                    case HandType.Left:
                        _startingObjectValue = _heldObject.transform.eulerAngles;
                        break;
                    case HandType.Right:
                        _startingObjectValue = _heldObject.transform.position;
                        break;
                }
            }
            _pinching = true;
        };
        _pinchValueAction.action.canceled += x => _pinching = false;
        _leftSecondaryAction.action.performed += x =>
        {
            if (!_pinching)
                return;
        
            _snapMode = _snapMode > 3 ? 0 : _snapMode + 1;
            _uiManager.UpdateSnapText(_handType, _snapMode);
        };

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

        if (_counterpartController.GetIsPinching())
        {
            float distance = Vector3.Distance(transform.position, _counterpartController.transform.position);
            _heldObject.ScaleObjectByAmount((distance - _lastDistance) * _pinchValueAction.action.ReadValue<float>() * Time.deltaTime * _scaleSpeed);
            _lastDistance = distance;
            _lastPinchPosition = _handPosAction.action.ReadValue<Vector3>();
            return;
        }

        switch (_handType)
        {
            case HandType.Left:
                if (_snapMode != 0)
                {
                    int index = 0;
                    if (_snapMode == 1)
                    {
                        Vector3 positionDifference = _handPosAction.action.ReadValue<Vector3>() - _startingPosition;
                        index = IsolateLargestIndexInVector(positionDifference);
                    }
                    else
                        index = _snapMode - 2;

                    _heldObject.RotateAmountInDirection(IsolateIndexInVector(_handPosAction.action.ReadValue<Vector3>(), index) - IsolateIndexInVector(_lastPinchPosition, index), _pinchValueAction.action.ReadValue<float>() * Time.deltaTime * _rotateSpeed);

                    if (_snapMode == 1)
                    {
                        if (_largestIndex != index)
                        {
                            _heldObject.transform.eulerAngles = _startingObjectValue;
                            _heldObject.RotateAmountInDirection(IsolateIndexInVector(_handPosAction.action.ReadValue<Vector3>(), index) - IsolateIndexInVector(_startingPosition, index), _pinchValueAction.action.ReadValue<float>() * Time.deltaTime * _rotateSpeed);
                        }
                        _largestIndex = index;
                    }
                }
                else
                    _heldObject.RotateAmountInDirection(_handPosAction.action.ReadValue<Vector3>() - _lastPinchPosition, _pinchValueAction.action.ReadValue<float>() * Time.deltaTime * _rotateSpeed);
                break;
            case HandType.Right:
                if (_snapMode != 0)
                {
                    int index = 0;
                    if (_snapMode == 1)
                    {
                        Vector3 positionDifference = _handPosAction.action.ReadValue<Vector3>() - _startingPosition;
                        index = IsolateLargestIndexInVector(positionDifference);
                    }
                    else
                        index = _snapMode - 2;

                    _heldObject.MoveAmountInDirection(IsolateIndexInVector(_handPosAction.action.ReadValue<Vector3>(), index) - IsolateIndexInVector(_lastPinchPosition, index), _pinchValueAction.action.ReadValue<float>() * Time.deltaTime * _moveSpeed);

                    if (_snapMode == 1)
                    {
                        if (_largestIndex != index)
                        {
                            _heldObject.transform.position = _startingObjectValue;
                            _heldObject.transform.position = _startingObjectValue + (IsolateIndexInVector(_handPosAction.action.ReadValue<Vector3>(), index) - IsolateIndexInVector(_startingPosition, index)) * _pinchValueAction.action.ReadValue<float>() * Time.deltaTime * _moveSpeed;
                        }
                        _largestIndex = index;
                    }
                }
                else
                    _heldObject.MoveAmountInDirection(_handPosAction.action.ReadValue<Vector3>() - _lastPinchPosition, _pinchValueAction.action.ReadValue<float>() * Time.deltaTime * _moveSpeed);
                break;
        }

        _lastPinchPosition = _handPosAction.action.ReadValue<Vector3>();
    }

    public bool GetIsPinching()
    {
        return _pinching;
    }

    private void SelectionChangedHandler(Object selection)
    {
        _heldObject = selection;
    }

    private Vector3 IsolateIndexInVector(Vector3 startingVector, int index)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i != index)
                startingVector[i] = 0f;
        }
        return startingVector;
    }

    private int IsolateLargestIndexInVector(Vector3 originalVector)
    {
        int index = 0;
        float largestValue = 0;
        for (int i = 0; i < 3; i++)
        {
            if (Mathf.Abs(originalVector[i]) > Mathf.Abs(largestValue))
            {
                largestValue = originalVector[i];
                index = i;
            }
        }

        return index;
    }
}

public enum HandType
{
    Left,
    Right
}
