using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [SerializeField]
    private Image _colorImage, _colorPreviewImage;

    public void UpdateColorPreview(Vector3 hitPosition)
    {
        Vector3 localPos = _colorImage.rectTransform.InverseTransformPoint(hitPosition);
        Vector2 imageSize = RectTransformUtility.CalculateRelativeRectTransformBounds(_colorImage.rectTransform).size;
        Color color = _colorImage.sprite.texture.GetPixel(Mathf.RoundToInt(localPos.x + imageSize.x / 2), Mathf.RoundToInt(localPos.y + imageSize.y / 2));
        _colorPreviewImage.color = color;
    }

    public Color GetColor(Vector3 hitPosition)
    {
        Vector3 localPos = _colorImage.rectTransform.InverseTransformPoint(hitPosition);
        Vector2 imageSize = RectTransformUtility.CalculateRelativeRectTransformBounds(_colorImage.rectTransform).size;
        Color color = _colorImage.sprite.texture.GetPixel(Mathf.RoundToInt(localPos.x + imageSize.x / 2), Mathf.RoundToInt(localPos.y + imageSize.y / 2));
        return color;
    }
}