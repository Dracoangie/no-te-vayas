using UnityEngine;

public class CameraSway : MonoBehaviour
{
    // Adds gentle movement and rotation to the camera.
    [SerializeField] private float speed = 0.4f;
    [SerializeField] private float movement = 0.015f;
    [SerializeField] private float rotation = 0.3f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float elapsed;

    private void OnEnable()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        elapsed = 0f;
    }

    private void LateUpdate()
    {
        elapsed += Time.unscaledDeltaTime * speed;
        float horizontal = Mathf.Sin(elapsed);
        float vertical = Mathf.Sin(elapsed * 0.7f);

        transform.localPosition = startPosition + new Vector3(horizontal, vertical, 0f) * movement;
        transform.localRotation = startRotation * Quaternion.Euler(vertical * rotation, horizontal * rotation, 0f);
    }

    private void OnDisable()
    {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }
}
