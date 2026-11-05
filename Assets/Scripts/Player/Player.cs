using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField, Min(0.01f)] private float accelerationTime = 0.25f;
    [SerializeField, Min(0.01f)] private float decelerationTime = 0.15f;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private Transform cameraPivot;
    [Header("Final")]
    [SerializeField] private Transform dragTarget;
    [SerializeField, Min(0.01f)] private float dragDuration = 2.8f;
    [SerializeField] private UnityEvent onDragFinished = new UnityEvent();

    private Rigidbody rb;
    private InputAction movementAction;
    private InputAction cameraAction;

    private Vector2 movement;
    private float yaw;
    private float pitch;
    private bool controlsEnabled = true;
    private bool dragging;
    private bool dragStarted;
    private bool wasKinematic;
    private Vector3 dragStart;
    private Vector3 dragEnd;
    private float dragElapsed;

    public bool ControlsEnabled => controlsEnabled;

    public void SetControlsEnabled(bool value)
    {
        controlsEnabled = value;
        movement = Vector2.zero;
        UpdateCursor();

        if (!value && !rb.isKinematic)
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
    }

    public void ArrastrarAlSalon()
    {
        if (dragStarted || dragTarget == null)
            return;

        if (InspectionController.Instance != null)
            InspectionController.Instance.Close();
        SetControlsEnabled(false);
        dragStarted = true;
        dragging = true;
        dragElapsed = 0f;
        dragStart = rb.position;
        dragEnd = dragTarget.position;
        dragEnd.y = dragStart.y;
        wasKinematic = rb.isKinematic;
        rb.isKinematic = true;
    }

    public void StopDragging()
    {
        if (!dragging)
            return;
        dragging = false;
        rb.isKinematic = wasKinematic;
        if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        movementAction = InputSystem.actions.FindAction("InGame/Movement", true);
        cameraAction = InputSystem.actions.FindAction("InGame/Camera", true);

        yaw = rb.rotation.eulerAngles.y;
    }

    private void Start()
    {
        UpdateCursor();
    }

    private void Update()
    {
        if (!controlsEnabled)
            return;

        movement = Vector2.ClampMagnitude( movementAction.ReadValue<Vector2>(), 1f);

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector2 look = cameraAction.ReadValue<Vector2>();
        yaw += look.x * mouseSensitivity;
        pitch -= look.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -85f, 85f);
    }

    private void FixedUpdate()
    {
        if (dragging)
        {
            dragElapsed += Time.fixedDeltaTime;
            float progress = Mathf.Clamp01(dragElapsed / Mathf.Max(0.01f, dragDuration));
            rb.MovePosition(Vector3.Lerp(dragStart, dragEnd, Mathf.SmoothStep(0f, 1f, progress)));
            if (progress >= 1f)
            {
                rb.position = dragEnd;
                StopDragging();
                onDragFinished.Invoke();
            }
            return;
        }

        if (!controlsEnabled)
            return;

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
    private void LateUpdate()
    {
        UpdateCursor();

        if (!controlsEnabled)
            return;

        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void UpdateCursor()
    {
        if (!Application.isFocused)
            return;

        CursorLockMode targetLock = controlsEnabled || dragging ? CursorLockMode.Locked : CursorLockMode.None;

        if (Cursor.lockState != targetLock)
            Cursor.lockState = targetLock;

        Cursor.visible = !controlsEnabled && !dragging;
    }

    private void OnDisable() => StopDragging();
}
