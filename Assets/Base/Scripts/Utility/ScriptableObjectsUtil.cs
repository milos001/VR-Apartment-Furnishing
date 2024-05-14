using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectsUtil
{
    public static Sprite GetPreviewFromGameobject(GameObject gameObject)
    {
        Texture2D tex = AssetPreview.GetAssetPreview(gameObject);
        Rect rec = new Rect(0, 0, tex.width, tex.height);
        return Sprite.Create(tex, rec, new Vector2(0, 0), 1);
    }
}
