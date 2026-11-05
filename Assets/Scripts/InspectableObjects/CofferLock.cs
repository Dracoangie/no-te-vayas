using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CofferLock : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition item;
    [SerializeField] private Vector3Int combination = new Vector3Int(0, 1, 2);
    [SerializeField] private Texture2D[] images;
    [SerializeField] private GameObject lockModel;
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private Transform lid;
    [SerializeField] private Vector3 lidOpenOffset = new Vector3(0f, 0.12f, -0.2f);
    [SerializeField, Min(0.01f)] private float lidSpeed = 0.4f;
    [SerializeField] private GameObject keyModel;
    [SerializeField] private RawImage[] wheelImages;
    [SerializeField] private GameObject thirdWheel;
    [SerializeField] private GameObject blockedThirdWheel;
    [SerializeField] private UnityEvent onUnlocked = new UnityEvent();
    [SerializeField] private UnityEvent onInteracted = new UnityEvent();
    [SerializeField] private UnityEvent onKeyCollected = new UnityEvent();

    private readonly int[] selected = new int[3];
    private CofferLock source;
    private bool unlocked;
    private bool thirdWheelEnabled;
    private bool keyCollected;
    private Vector3 closedLidPosition;
    private CofferLock State => source != null ? source : this;
    public bool IsUnlocked => State.unlocked;
    public bool IsKeyCollected => State.keyCollected;
    public bool IsPanelOpen => lockPanel != null && lockPanel.activeSelf;

    private void Awake()
    {
        if (lid != null)
            closedLidPosition = lid.localPosition;
        SetPanelOpen(false);
        UpdateWheel();
        UpdateContents(true);
    }

    private void Update()
    {
        if (source != null)
            UpdateWheel();
        UpdateContents();
    }

    private void UpdateContents(bool snap = false)
    {
        if (lid != null)
        {
            Vector3 target = closedLidPosition + (IsUnlocked ? lidOpenOffset : Vector3.zero);
            lid.localPosition = snap ? target
                : Vector3.MoveTowards(lid.localPosition, target, lidSpeed * Time.deltaTime);
        }
        if (keyModel != null)
            keyModel.SetActive(IsUnlocked && !IsKeyCollected);
    }

    public void CollectKey()
    {
        if (!IsUnlocked || IsKeyCollected)
            return;

        State.keyCollected = true;
        State.UpdateContents();
        UpdateContents();
        State.onKeyCollected.Invoke();
    }

    public void EnableThirdWheel()
    {
        State.thirdWheelEnabled = true;
        State.UpdateWheel();
        UpdateWheel();
    }

    private void UpdateWheel()
    {
        bool active = State.thirdWheelEnabled;
        if (thirdWheel != null)
            thirdWheel.SetActive(active);
        if (blockedThirdWheel != null)
            blockedThirdWheel.SetActive(!active);
    }

    public void PreviousImage(int wheel) => MoveWheel(wheel, -1);
    public void NextImage(int wheel) => MoveWheel(wheel, 1);
    public void ClosePanel() => SetPanelOpen(false);

    public void Interact()
    {
        onInteracted.Invoke();

        if (InspectionController.Instance != null)
            InspectionController.Instance.Open(item, this);
    }

    public void PreparePreview(CofferLock original)
    {
        source = original;
        UpdateWheel();
        SetPanelOpen(false);
        SetPreviewUnlocked(IsUnlocked);
        UpdateContents(true);
    }

    public void SetPanelOpen(bool open)
    {
        if (lockPanel == null)
            return;
        lockPanel.SetActive(open);
        if (open)
        {
            UpdateImages();
            UpdateWheel();
        }
    }

    public void SetPreviewUnlocked(bool isUnlocked)
    {
        if (lockModel != null)
            lockModel.SetActive(!isUnlocked);
    }

    private void MoveWheel(int wheel, int direction)
    {
        if (wheel < 0 || wheel >= selected.Length || IsUnlocked || State.images == null || State.images.Length == 0)
            return;
        if (wheel == 2 && !State.thirdWheelEnabled)
            return;
        State.selected[wheel] = (State.selected[wheel] + direction + State.images.Length) % State.images.Length;
        UpdateImages();
    }

    private void UpdateImages()
    {
        for (int i = 0; i < 3; i++)
            wheelImages[i].texture = State.images != null && State.images.Length > 0
                ? State.images[State.selected[i] % State.images.Length] : null;
    }

    public void TryUnlock()
    {
        if (IsUnlocked)
            return;
        if (!State.thirdWheelEnabled)
            return;
        if (State.selected[0] != State.combination.x || State.selected[1] != State.combination.y ||
            State.selected[2] != State.combination.z)
            return;
        State.unlocked = true;
        State.SetPreviewUnlocked(true);
        SetPreviewUnlocked(true);
        ClosePanel();
        State.onUnlocked.Invoke();
    }

    public bool InteractWithPreview(Ray ray)
    {
        Physics.SyncTransforms();
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,
            1 << gameObject.layer, QueryTriggerInteraction.Ignore))
            return false;

        if (!IsUnlocked && lockModel != null && hit.transform.IsChildOf(lockModel.transform))
        {
            SetPanelOpen(true);
            return true;
        }
        if (IsUnlocked && !IsKeyCollected && keyModel != null && hit.transform.IsChildOf(keyModel.transform))
        {
            CollectKey();
            return true;
        }
        return false;
    }
}
