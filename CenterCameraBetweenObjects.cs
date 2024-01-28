using UnityEngine;

public class CenterCameraBetweenObjects : MonoBehaviour
{
    public Transform object1;
    public Transform object2;
    public float minX = -2.0f; // Minimum X position for the camera.
    public float maxX = 2.0f; // Maximum X position for the camera.
    public float smoothness = 5.0f; // Smoothing factor for camera movement.

    private Camera mainCamera;

    private void Start()
    {
        // Get the main camera in the scene.
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera != null && object1 != null && object2 != null)
        {
            // Calculate the center point between the two objects.
            Vector3 centerPoint = (object1.position + object2.position) / 2.0f;

            // Clamp the center point within the specified range.
            centerPoint.x = Mathf.Clamp(centerPoint.x, minX, maxX);

            // Calculate the target camera position with smoothing.
            Vector3 targetPosition = new Vector3(centerPoint.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * smoothness);
        }
    }
}
