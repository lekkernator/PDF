using UnityEngine;

public class SmoothMovement : MonoBehaviour
{
    public float delta = 0.3f;  // Amount to move up and down from the start point
    public float speed = 2.0f;  // Speed of the movement

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 v = startPosition;
        v.y += delta * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
}