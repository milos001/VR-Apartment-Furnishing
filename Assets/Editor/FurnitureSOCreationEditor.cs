using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class FurnitureSOCreationEditor : EditorWindow
{
    [SerializeField]
    private GameObject[] _prefabs;

    [MenuItem("Window/FurnitureSOCreationEditor")]
    public static void ShowWindow()
    {
        GetWindow<FurnitureSOCreationEditor>();
    }

    private void OnGUI()
    {
        ScriptableObject scriptableObj = this;
        SerializedObject serialObj = new SerializedObject(scriptableObj);
        SerializedProperty serialProp = serialObj.FindProperty("_prefabs");

        EditorGUILayout.PropertyField(serialProp, true);
        serialObj.ApplyModifiedProperties();

        if (GUILayout.Button("CreateObject"))
            CreateScriptableObjects();
    }

    private void CreateScriptableObjects()
    {
        foreach (var prefab in _prefabs)
        {
            FurnitureSO newItem = CreateInstance<FurnitureSO>();
            newItem.name = prefab.name;
            newItem.Prefab = prefab;

            Texture2D tex = AssetPreview.GetAssetPreview(prefab);
            tex.name = prefab.name;
            SaveSubSprite(tex, "Assets/Base/SOs/Previews/");
            Texture2D loadedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Base/SOs/Previews/" + tex.name + ".png");

            newItem.Preview = loadedTexture;
            AssetDatabase.CreateAsset(newItem, "Assets/Base/SOs/" + prefab.name + ".asset");
        }
    }

    private static void SaveSubSprite(Texture2D tex, string saveToDirectory)
    {
        if (!System.IO.Directory.Exists(saveToDirectory)) System.IO.Directory.CreateDirectory(saveToDirectory);
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(saveToDirectory, tex.name + ".png"), tex.EncodeToPNG());
    }
}