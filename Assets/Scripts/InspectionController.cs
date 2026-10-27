using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InspectionController : MonoBehaviour
{
    public static InspectionController Instance { get; private set; }

    [SerializeField] private GameObject inspectionPanel;
    [FormerlySerializedAs("inspectionCamera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform modelPivot;
    [SerializeField] private Player player;
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float zoomSpeed = 0.002f;
    [SerializeField] private float initialDistance = 0.7f;
    [SerializeField] private float minDistance = 0.4f;
    [SerializeField] private float maxDistance = 1.2f;

    private GameObject currentModel;
    private bool isOpen;
    private float distance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Solo debe haber un InspectionController en la escena.", this);
            enabled = false;
            return;
        }

        if (inspectionPanel == null || playerCamera == null || modelPivot == null || player == null)
        {
            Debug.LogError("Asigna las referencias del visor en InspectionController.", this);
            enabled = false;
            return;
        }

        Instance = this;
        inspectionPanel.SetActive(false);
    }

    public void Open(ItemDefinition item)
    {
        if (!isActiveAndEnabled || isOpen || item == null || item.InspectionPrefab == null)
            return;

        currentModel = Instantiate(item.InspectionPrefab, modelPivot);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.Euler(item.InitialRotation);
        modelPivot.rotation = playerCamera.transform.rotation;

        distance = Mathf.Clamp(initialDistance, minDistance, maxDistance);
        UpdateModelPosition();

        player.SetControlsEnabled(false);

        inspectionPanel.SetActive(true);
        isOpen = true;
    }

    private void Update()
    {
        if (!isOpen)
            return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Close();
            return;
        }

        Mouse mouse = Mouse.current;
        if (mouse == null)
            return;

        if (mouse.leftButton.isPressed)
        {
            Vector2 delta = mouse.delta.ReadValue();
            modelPivot.Rotate(playerCamera.transform.up, -delta.x * rotationSpeed, Space.World);
            modelPivot.Rotate(playerCamera.transform.right, delta.y * rotationSpeed, Space.World);
        }

        distance = Mathf.Clamp(distance - mouse.scroll.ReadValue().y * zoomSpeed,
            minDistance, maxDistance);
    }

    private void LateUpdate()
    {
        if (isOpen)
            UpdateModelPosition();

    }

    private void UpdateModelPosition()
    {
        modelPivot.position = playerCamera.transform.position + playerCamera.transform.forward * distance;
    }

    public void Close()
    {
        if (!isOpen)
            return;

        Destroy(currentModel);
        inspectionPanel.SetActive(false);
        player.SetControlsEnabled(true);
        isOpen = false;
    }

    private void OnDisable()
    {
        Close();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
