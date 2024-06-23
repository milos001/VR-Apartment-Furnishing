using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class HandMovementController : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTs, _cameraTs;
    [SerializeField]
    private InputActionProperty _axisAction;
    [SerializeField]
    private HandType _handType;

    private float _moveSpeed = 3.5f, _rotateDegrees = 5f;
    private bool _axisChanged;

    // Start is called before the first frame update
    async void Start()
    {
        switch (_handType)
        {
            case HandType.Left:
                _axisAction.action.performed += RotateCameraInDirection;
                break;
            case HandType.Right:
                _axisAction.action.started += x => _axisChanged = true;
                _axisAction.action.canceled += x => _axisChanged = false;
                break;
        }

        List<Vector3> points = new List<Vector3>();
        while (XRGeneralSettings.Instance.Manager.activeLoader == null || !XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRInputSubsystem>().TryGetBoundaryPoints(points))
        {
            await Task.Delay(100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_handType == HandType.Right && _axisChanged)
        {
            Vector2 input = _axisAction.action.ReadValue<Vector2>();
            Vector3 moveDir = _cameraTs.TransformDirection(new Vector3(input.x, 0f, input.y));
            moveDir.y = 0f;
            _playerTs.Translate(moveDir * _moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void RotateCameraInDirection(InputAction.CallbackContext callbackContext)
    {
        Vector2 input = callbackContext.ReadValue<Vector2>();
        _playerTs.eulerAngles = new Vector3(_playerTs.eulerAngles.x, _playerTs.eulerAngles.y + _rotateDegrees * input.x, _playerTs.eulerAngles.z);
    }
}
