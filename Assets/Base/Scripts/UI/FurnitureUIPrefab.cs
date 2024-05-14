using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUIPrefab : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private Image _previewImage;

    private UIManager _manager;
    private FurnitureSO _data;

    public void Initialize(FurnitureSO data, UIManager manager)
    {
        _data = data;
        _manager = manager;

        _nameText.text = data.name;
        Texture2D tex = data.Preview;
        Rect rec = new Rect(0, 0, tex.width, tex.height);
        _previewImage.sprite = Sprite.Create(tex, rec, new Vector2(0, 0), 1);
    }

    public void OnButtonPressed()
    {
        _manager.SpawnObject(_data);
    }
}
