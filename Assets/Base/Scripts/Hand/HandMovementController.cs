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
    private InputActionProperty _axisAction, _joystickAction;
    [SerializeField]
    private HandType _handType;

    private float _moveSpeed = 10f, _ySpeed = 5f;
    private bool _axisChanged, _joystickHeld;

    // Start is called before the first frame update
    async void Start()
    {
        _axisAction.action.started += x => _axisChanged = true;
        _axisAction.action.canceled += x => _axisChanged = false;
        _joystickAction.action.started += x => _joystickHeld = true;
        _joystickAction.action.canceled += x => _joystickHeld = false;

        List<Vector3> points = new List<Vector3>();
        while (XRGeneralSettings.Instance.Manager.activeLoader == null || !XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRInputSubsystem>().TryGetBoundaryPoints(points))
        {
            await Task.Delay(100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_axisChanged)
        {
            Vector2 input = _axisAction.action.ReadValue<Vector2>();
            Vector3 moveDir = _cameraTs.TransformDirection(new Vector3(input.x, 0f, input.y));
            moveDir.y = 0f;
            _playerTs.Translate(moveDir * _moveSpeed * Time.deltaTime, Space.World);
        }

        if (_joystickHeld)
        {
            switch (_handType)
            {
                case HandType.Right:
                    _playerTs.Translate(Vector3.up * _ySpeed * Time.deltaTime, Space.Self);
                    break;
                case HandType.Left:
                    _playerTs.Translate(Vector3.down * _ySpeed * Time.deltaTime, Space.Self);
                    break;
            }
        }
    }
}
