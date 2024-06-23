using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _westWall, _eastWall, _northWall, _southWall, _ceiling;

    private float _baseThickness = .05f;
    private float _currentWidth, _currentLength, _currentHeight;

    public void ChangeRoomWidth(float value)
    {
        _westWall.transform.position = new Vector3(-value / 2, _currentHeight / 2f, 0);
        _eastWall.transform.position = new Vector3(value / 2, _currentHeight / 2f, 0);
        _northWall.transform.localScale = new Vector3(_baseThickness, _currentHeight, value);
        _southWall.transform.localScale = new Vector3(_baseThickness, _currentHeight, value);
        _ceiling.transform.localScale = new Vector3(_baseThickness, value, _currentLength);
        _currentWidth = value;
    }

    public void ChangeRoomLength(float value)
    {
        _northWall.transform.position = new Vector3(0, _currentHeight / 2f, value / 2);
        _southWall.transform.position = new Vector3(0, _currentHeight / 2f, -value / 2);
        _westWall.transform.localScale = new Vector3(_baseThickness, _currentHeight, value);
        _eastWall.transform.localScale = new Vector3(_baseThickness, _currentHeight, value);
        _ceiling.transform.localScale = new Vector3(_baseThickness, _currentWidth, value);
        _currentLength = value;
    }

    public void ChangeRoomHeight(float value)
    {
        _ceiling.transform.position = new Vector3(0, value, 0);
        _currentHeight = value;
        ChangeRoomLength(_currentLength);
        ChangeRoomWidth(_currentWidth);
    }

    public void ChangeRoomColor(Color color)
    {
        _westWall.material.color = color;
        _eastWall.material.color = color;
        _northWall.material.color = color;
        _southWall.material.color = color;
        _ceiling.material.color = color;
    }
}
