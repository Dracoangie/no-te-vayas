using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField, Min(0.01f)] private float accelerationTime = 0.25f;
    [SerializeField, Min(0.01f)] private float decelerationTime = 0.15f;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private Transform cameraPivot;

    private Rigidbody rb;
    private InputAction movementAction;
    private InputAction cameraAction;

    private Vector2 movement;
    private float yaw;
    private float pitch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
                
        movementAction = InputSystem.actions.FindAction("InGame/Movement", true);
        cameraAction = InputSystem.actions.FindAction("InGame/Camera", true);

        yaw = rb.rotation.eulerAngles.y;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        movement = Vector2.ClampMagnitude( movementAction.ReadValue<Vector2>(), 1f);

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector2 look = cameraAction.ReadValue<Vector2>();
        yaw += look.x * mouseSensitivity;
        pitch -= look.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -85f, 85f);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        rb.MoveRotation(rotation);

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 targetVelocity = (rotation * new Vector3(movement.x, 0f, movement.y)) * speed;

        float smoothingTime = movement.sqrMagnitude > 0f
            ? accelerationTime
            : decelerationTime;
        float acceleration = speed / Mathf.Max(smoothingTime, 0.01f);

        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
    }
}
