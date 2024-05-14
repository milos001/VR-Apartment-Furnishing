using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private InputActionProperty _pinchAction, _grabAction;

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Trigger", _pinchAction.action.ReadValue<float>());
        _animator.SetFloat("Grip", Mathf.Clamp(_grabAction.action.ReadValue<float>(), 0f, .5f));
    }
} 