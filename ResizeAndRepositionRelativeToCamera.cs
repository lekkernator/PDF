using UnityEngine;

public class TrackCameraPosition : MonoBehaviour
{
    public float offsetX = 1.0f; // Adjust this value to control the X position relative to the camera.
    public float offsetY = 1.0f; // Adjust this value to control the Y position relative to the camera.

    private Camera mainCamera;

    private void Start()
    {
        // Get the main camera in the scene.
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            // Get the current camera position.
            Vector3 cameraPosition = mainCamera.transform.position;

            // Calculate the new position relative to the camera.
            Vector3 newPosition = new Vector3(cameraPosition.x + offsetX, cameraPosition.y + offsetY, transform.position.z);

            // Set the new position for the object.
            transform.position = newPosition;
        }
    }
}
