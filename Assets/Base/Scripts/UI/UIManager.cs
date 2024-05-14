using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
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
    private InputActionProperty _leftPrimaryInputAction;

    private Vector3 _offset = new Vector3(0f, 0f, 1.5f);

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
        transform.position = _cameraTs.position + _offset;
    }

    public void SpawnObject(FurnitureSO data)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0f;
        gameObject.SetActive(false);
        Object spawnedObject = Instantiate(data.Prefab, spawnPos, Quaternion.identity).AddComponent<Object>();
        spawnedObject.gameObject.layer = LayerMask.NameToLayer("Object");
    }
}
