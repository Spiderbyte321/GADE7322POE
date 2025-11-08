using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShadedMaterialRandomiser : MonoBehaviour
{
    [SerializeField] private Material[] ShadedMaterials;
    [SerializeField] private MeshRenderer Renderer;

    private void Awake()
    {
        Renderer.material = ShadedMaterials[Random.Range(0, ShadedMaterials.Length - 1)];
    }
}
