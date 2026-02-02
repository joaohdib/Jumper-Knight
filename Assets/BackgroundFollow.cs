using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [Header("Settings")]
    public Transform cameraTransform;
    public Vector2 offset;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        // keep the background centered on the camera's X and Y positions
        // we maintain the background's original Z position
        Vector3 newPosition = new Vector3(
            cameraTransform.position.x + offset.x, 
            cameraTransform.position.y + offset.y, 
            transform.position.z
        );

        transform.position = newPosition;
    }
}