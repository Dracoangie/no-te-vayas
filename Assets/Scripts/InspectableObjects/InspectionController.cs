using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InspectionController : MonoBehaviour
{
    public static InspectionController Instance { get; private set; }

    [SerializeField] private GameObject inspectionPanel;
    [SerializeField] private Camera inspectionCamera;
    [SerializeField] private RawImage inspectionImage;
    [SerializeField] private Transform modelPivot;
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float zoomSpeed = 0.002f;
    [SerializeField] private float initialDistance = 0.7f;
    [SerializeField] private float minDistance = 0.4f;
    [SerializeField] private float maxDistance = 1.2f;
    [SerializeField, Min(128)] private int textureSize = 1024;
    [SerializeField, Min(0.01f)] private float modelRadius = 0.18f;

    private GameObject model;
    private CofferLock coffer;
    private CofferLock preview;
    private Player player;
    private RenderTexture texture;
    private bool isOpen;
    private Vector2 clickStart;
    private bool isClick;
    private float distance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("The scene must contain only one InspectionController.", this);
            enabled = false;
            return;
        }

        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("The scene must contain a Player for inspection.", this);
            enabled = false;
            return;
        }

        if (inspectionPanel == null || inspectionCamera == null ||
            inspectionImage == null || modelPivot == null)
        {
            Debug.LogError("Assign all preview references in InspectionController.", this);
            enabled = false;
            return;
        }

        Instance = this;
        texture = new RenderTexture(textureSize, textureSize, 24, RenderTextureFormat.ARGBHalf);
        texture.name = "Inspection Preview";
        texture.Create();
        inspectionCamera.targetTexture = texture;
        inspectionCamera.aspect = 1f;
        inspectionImage.texture = texture;
        inspectionCamera.enabled = false;
        inspectionPanel.SetActive(false);
    }

    public void Open(ItemDefinition item, CofferLock cofferLock = null)
    {
        if (!isActiveAndEnabled || isOpen || item == null || item.InspectionPrefab == null)
            return;

        modelPivot.rotation = inspectionCamera.transform.rotation;
        coffer = cofferLock;
        isClick = false;
        model = Instantiate(item.InspectionPrefab, modelPivot);
        if (model.TryGetComponent(out ItemDescriptionText text))
            text.SetItem(item);
        preview = model.GetComponent<CofferLock>();
        if (preview != null)
            preview.PreparePreview(coffer);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(item.InitialRotation);
        SetupModel();

        distance = Mathf.Clamp(initialDistance, minDistance, maxDistance);
        MoveModel();

        player.SetControlsEnabled(false);

        inspectionPanel.SetActive(true);
        inspectionCamera.enabled = true;
        isOpen = true;
    }

    private void SetupModel()
    {
        foreach (Transform child in model.GetComponentsInChildren<Transform>(true))
            child.gameObject.layer = inspectionCamera.gameObject.layer;

        foreach (Collider collider in model.GetComponentsInChildren<Collider>(true))
            collider.enabled = preview != null && collider is MeshCollider;

        foreach (Rigidbody body in model.GetComponentsInChildren<Rigidbody>(true))
        {
            body.isKinematic = true;
            body.detectCollisions = false;
        }

        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return;

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);

        float scale = modelRadius / Mathf.Max(bounds.extents.magnitude, 0.001f);
        Vector3 offset = bounds.center - modelPivot.position;
        model.transform.localScale *= scale;
        model.transform.position = modelPivot.position - offset * scale;
    }

    private void Update()
    {
        if (!isOpen)
            return;

        bool lockPanelOpen = preview != null && preview.IsPanelOpen;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (lockPanelOpen)
                preview.SetPanelOpen(false);
            else
                Close();
            return;
        }

        Mouse mouse = Mouse.current;
        if (mouse == null)
            return;

        if (lockPanelOpen)
            return;

        Vector2 pointer = mouse.position.ReadValue();
        if (mouse.leftButton.wasPressedThisFrame)
        {
            clickStart = pointer;
            isClick = true;
        }
        if (isClick && Vector2.Distance(pointer, clickStart) > 6f)
            isClick = false;
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            bool clicked = isClick;
            isClick = false;
            if (clicked && CheckClick(pointer))
            {
                return;
            }
        }

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
            MoveModel();
    }

    private bool CheckClick(Vector2 screenPoint)
    {
        if (coffer == null || preview == null)
            return false;

        Canvas canvas = inspectionImage.canvas;
        Camera uiCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? canvas.worldCamera : null;
        RectTransform imageRect = inspectionImage.rectTransform;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRect, screenPoint,
            uiCamera, out Vector2 localPoint) || !imageRect.rect.Contains(localPoint))
            return false;

        Rect rect = imageRect.rect;
        Rect uv = inspectionImage.uvRect;
        Vector2 viewport = new Vector2(
            uv.x + (localPoint.x - rect.x) / rect.width * uv.width,
            uv.y + (localPoint.y - rect.y) / rect.height * uv.height);
        Ray ray = inspectionCamera.ViewportPointToRay(viewport);
        return preview.InteractWithPreview(ray);
    }

    private void MoveModel()
    {
        modelPivot.position = inspectionCamera.transform.position + inspectionCamera.transform.forward * distance;
    }

    public void Close()
    {
        if (!isOpen)
            return;

        Destroy(model);
        coffer = null;
        preview = null;
        isClick = false;
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
        if (texture != null)
        {
            if (inspectionCamera != null)
                inspectionCamera.targetTexture = null;
            if (inspectionImage != null)
                inspectionImage.texture = null;
            texture.Release();
            Destroy(texture);
        }

        if (Instance == this)
            Instance = null;
    }
}
