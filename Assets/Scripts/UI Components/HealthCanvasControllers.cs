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
        gameObject.transform.LookAt(mainCamera.transform);
    }
}
