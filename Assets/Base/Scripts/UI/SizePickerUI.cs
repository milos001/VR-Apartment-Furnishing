using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SizePickerUI : MonoBehaviour
{
    [SerializeField]
    private Slider _widthSlider, _lengthSlider, _heightSlider;

    [SerializeField]
    private TMP_Text _widthText, _lengthText, _heightText;

    [SerializeField]
    private RoomController _roomController;

    // Start is called before the first frame update
    void Start()
    {
        _widthSlider.onValueChanged.AddListener(WidthChanged);
        _lengthSlider.onValueChanged.AddListener(LengthChanged);
        _heightSlider.onValueChanged.AddListener(HeightChanged);

        _heightSlider.onValueChanged.Invoke(_heightSlider.value);
        _widthSlider.onValueChanged.Invoke(_widthSlider.value);
        _lengthSlider.onValueChanged.Invoke(_lengthSlider.value);
    }

    private void WidthChanged(float value)
    {
        _roomController.ChangeRoomWidth(value);
        _widthText.text = (int) (value * 10) / 10f + "";
    }

    private void LengthChanged(float value)
    {
        _roomController.ChangeRoomLength(value);
        _lengthText.text = (int)(value * 10) / 10f + "";
    }

    private void HeightChanged(float value)
    {
        _roomController.ChangeRoomHeight(value);
        _heightText.text = (int)(value * 10) / 10f + "";
    }
}
