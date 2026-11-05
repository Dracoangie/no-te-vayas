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
    [Header("Al caminar")]
    [SerializeField] private Transform walkCamera;
    [SerializeField] private AudioSource footsteps;
    [SerializeField, Min(0f)] private float walkShakeAmount = 0.018f;
    [SerializeField, Min(0f)] private float walkShakeFrequency = 1.8f;
    [SerializeField, Min(0f)] private float walkShakeRotation = 0.35f;
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
    private CapsuleCollider capsule;
    private Vector3 cameraRestPosition;
    private Quaternion cameraRestRotation;
    private float walkPhase;

    public bool ControlsEnabled => controlsEnabled;

    public void SetControlsEnabled(bool value)
    {
        controlsEnabled = value;
        movement = Vector2.zero;
        UpdateCursor();

        if (!value && !rb.isKinematic)
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        if (!value)
            ResetWalkingEffects();
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
        capsule = GetComponent<CapsuleCollider>();
        if (walkCamera != null)
        {
            cameraRestPosition = walkCamera.localPosition;
            cameraRestRotation = walkCamera.localRotation;
        }
        if (footsteps != null)
        {
            footsteps.playOnAwake = false;
            footsteps.loop = true;
            footsteps.Stop();
        }
                
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
        UpdateWalkingEffects();

        if (!controlsEnabled)
            return;

        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void UpdateWalkingEffects()
    {
        Vector3 horizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        bool walking = controlsEnabled && !dragging && Time.timeScale > 0f &&
            movement.sqrMagnitude > 0.01f && horizontal.sqrMagnitude > 0.01f;
        if (walking && capsule != null)
            walking = Physics.Raycast(capsule.bounds.center, Vector3.down,
                capsule.bounds.extents.y + 0.15f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        if (!walking)
        {
            ResetWalkingEffects();
            return;
        }

        if (footsteps != null && footsteps.clip != null && !footsteps.isPlaying)
            footsteps.Play();

        walkPhase += Time.deltaTime * walkShakeFrequency * Mathf.PI * 2f;
        if (walkCamera != null)
        {
            float sway = Mathf.Sin(walkPhase);
            walkCamera.localPosition = cameraRestPosition +
                new Vector3(sway * 0.5f, Mathf.Sin(walkPhase * 2f), 0f) * walkShakeAmount;
            walkCamera.localRotation = cameraRestRotation *
                Quaternion.Euler(0f, 0f, sway * walkShakeRotation);
        }
    }

    private void ResetWalkingEffects()
    {
        walkPhase = 0f;
        if (footsteps != null && footsteps.isPlaying)
            footsteps.Stop();
        if (walkCamera != null)
        {
            walkCamera.localPosition = cameraRestPosition;
            walkCamera.localRotation = cameraRestRotation;
        }
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

    private void OnDisable()
    {
        StopDragging();
        ResetWalkingEffects();
    }
}
