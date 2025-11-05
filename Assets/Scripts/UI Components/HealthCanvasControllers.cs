using System;
using TMPro;
using UnityEngine;

public class HealthCanvasControllers : MonoBehaviour
{
    private Camera mainCamera;
    

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    

    void Update()
    {
        Quaternion CanvasFaceRotation = mainCamera.transform.rotation;
        gameObject.transform.rotation = CanvasFaceRotation;
    }
    
}
