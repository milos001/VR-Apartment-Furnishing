using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(FurnitureSO), menuName = nameof(FurnitureSO))]
public class FurnitureSO : ScriptableObject
{
    public GameObject Prefab;
    public Texture2D Preview;
}
