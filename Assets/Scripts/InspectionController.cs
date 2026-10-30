using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InspectionController : MonoBehaviour
{
    public static InspectionController Instance { get; private set; }

    [SerializeField] private GameObject inspectionPanel;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera inspectionCamera;
    [SerializeField] private RawImage inspectionImage;
    [SerializeField] private Transform modelPivot;
    [SerializeField] private Player player;
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float zoomSpeed = 0.002f;
    [SerializeField] private float initialDistance = 0.7f;
    [SerializeField] private float minDistance = 0.4f;
    [SerializeField] private float maxDistance = 1.2f;
    [SerializeField, Min(128)] private int textureSize = 1024;
    [SerializeField, Min(0.01f)] private float modelRadius = 0.18f;

    private GameObject currentModel;
    private RenderTexture inspectionTexture;
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

        if (inspectionPanel == null || playerCamera == null || inspectionCamera == null ||
            inspectionImage == null || modelPivot == null || player == null)
        {
            Debug.LogError("Asigna las referencias del visor en InspectionController.", this);
            enabled = false;
            return;
        }

        Instance = this;
        inspectionTexture = new RenderTexture(textureSize, textureSize, 24, RenderTextureFormat.ARGBHalf);
        inspectionTexture.name = "Inspection Preview";
        inspectionTexture.Create();
        inspectionCamera.targetTexture = inspectionTexture;
        inspectionCamera.aspect = 1f;
        inspectionImage.texture = inspectionTexture;
        inspectionCamera.enabled = false;
        inspectionPanel.SetActive(false);
    }

    public void Open(ItemDefinition item)
    {
        if (!isActiveAndEnabled || isOpen || item == null || item.InspectionPrefab == null)
            return;

        modelPivot.rotation = inspectionCamera.transform.rotation;
        currentModel = Instantiate(item.InspectionPrefab, modelPivot);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.Euler(item.InitialRotation);
        PrepareModel();

        distance = Mathf.Clamp(initialDistance, minDistance, maxDistance);
        UpdateModelPosition();

        player.SetControlsEnabled(false);

        inspectionPanel.SetActive(true);
        inspectionCamera.enabled = true;
        isOpen = true;
    }

    private void PrepareModel()
    {
        // Only the preview camera sees this copy, including meshes in child objects.
        foreach (Transform child in currentModel.GetComponentsInChildren<Transform>(true))
            child.gameObject.layer = inspectionCamera.gameObject.layer;

        foreach (Collider collider in currentModel.GetComponentsInChildren<Collider>(true))
            collider.enabled = false;

        foreach (Rigidbody body in currentModel.GetComponentsInChildren<Rigidbody>(true))
        {
            body.isKinematic = true;
            body.detectCollisions = false;
        }

        Renderer[] renderers = currentModel.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return;

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
            bounds.Encapsulate(renderer.bounds);

        // Center the mesh rather than assuming its imported pivot is centered.
        float scale = modelRadius / Mathf.Max(bounds.extents.magnitude, 0.001f);
        Vector3 centerOffset = bounds.center - modelPivot.position;
        currentModel.transform.localScale *= scale;
        currentModel.transform.position = modelPivot.position - centerOffset * scale;
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
            modelPivot.Rotate(inspectionCamera.transform.up, -delta.x * rotationSpeed, Space.World);
            modelPivot.Rotate(inspectionCamera.transform.right, delta.y * rotationSpeed, Space.World);
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
        modelPivot.position = inspectionCamera.transform.position + inspectionCamera.transform.forward * distance;
    }

    public void Close()
    {
        if (!isOpen)
            return;

        Destroy(currentModel);
        inspectionCamera.enabled = false;
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
        if (inspectionTexture != null)
        {
            if (inspectionCamera != null)
                inspectionCamera.targetTexture = null;
            if (inspectionImage != null)
                inspectionImage.texture = null;
            inspectionTexture.Release();
            Destroy(inspectionTexture);
        }

        if (Instance == this)
            Instance = null;
    }
}
