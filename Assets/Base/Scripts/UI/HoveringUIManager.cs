using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class HoveringUIManager : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTs;

    [SerializeField]
    private FurnitureSO[] _furnitureList;
    [SerializeField]
    private FurnitureUIPrefab _furnitureUIPrefab;
    [SerializeField]
    private RectTransform _furnitureListParent;
    [SerializeField]
    private Image _pointerImage;
    [SerializeField]
    private TMP_Text _leftSnapText, _rightSnapText; 
    [SerializeField]
    private RayInteractionController _rayInteractionController;
    [SerializeField]
    private ColorPicker _colorPicker;
    [SerializeField]
    private RoomController _roomController;

    [SerializeField]
    private InputActionProperty _leftPrimaryInputAction;

    private Vector3 _offset = new Vector3(0f, -1f, 1.5f);

    private void Start()
    {
        _leftPrimaryInputAction.action.performed += value => gameObject.SetActive(!gameObject.activeInHierarchy);

        foreach (var furnitureData in _furnitureList)
        {
            Instantiate(_furnitureUIPrefab, _furnitureListParent).Initialize(furnitureData, this);
        }
    }

    private void Update()
    {
        Vector3 newPos = _cameraTs.position + _offset;
        newPos.y = transform.position.y;
        transform.position = newPos;

        if (_rayInteractionController.IsOnUI())
        {
            Vector3 hitPos = _rayInteractionController.GetUIWorldPosition();
            _pointerImage.transform.position = hitPos;
            if (Vector2.Distance(hitPos, _colorPicker.transform.position) < .29f)
                _colorPicker.UpdateColorPreview(hitPos);
        }
    }

    public void SpawnObject(FurnitureSO data)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0f;
        gameObject.SetActive(false);
        Object spawnedObject = Instantiate(data.Prefab, spawnPos, Quaternion.identity).AddComponent<Object>();
        spawnedObject.gameObject.layer = LayerMask.NameToLayer("Object");
    }

    public void ChangeRoomColor()
    {
        if (_rayInteractionController.IsOnUI())
        {
            Vector3 hitPos = _rayInteractionController.GetUIWorldPosition();
            _pointerImage.transform.position = hitPos;
            _roomController.ChangeRoomColor(_colorPicker.GetColor(hitPos));
        }
    }

    public void UpdateSnapText(HandType hand, int snap)
    {
        string text = "";
        switch (snap)
        {
            case 0:
                text = "Free";
                break;
            case 1:
                text = "Axis";
                break;
            case 2:
                text = "X";
                break;
            case 3:
                text = "Y";
                break;
            case 4:
                text = "Z";
                break;
        }

        switch (hand)
        {
            case HandType.Left:
                _leftSnapText.text = text;
                break;
            case HandType.Right:
                _rightSnapText.text = text;
                break;
        }
    }
}
