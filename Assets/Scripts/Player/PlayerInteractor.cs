using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField, Min(0.1f)] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers = Physics.DefaultRaycastLayers;
    private GameObject interactionPanel;

    private InputAction interactionAction;
    private Player player;

    private void Awake()
    {
        interactionAction = InputSystem.actions.FindAction("InGame/Interaction", true);
        player = GetComponent<Player>();

        if (playerCamera == null)
        {
            Debug.LogError("Assign the player camera in PlayerInteractor.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        interactionPanel = GameObject.Find("Interaction Panel");
        if (interactionPanel != null)
            interactionPanel.SetActive(false);
    }

    private void Update()
    {
        IInteractable interactable = ActualizarInteraccion();
        if (interactable != null && interactionAction.WasPressedThisFrame())
            interactable.Interact();

        if (!player.ControlsEnabled && interactionPanel != null)
            interactionPanel.SetActive(false);
    }

    private IInteractable ActualizarInteraccion()
    {
        IInteractable interactable = null;
        if (player.ControlsEnabled && Time.timeScale > 0f)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance,
                interactionLayers, QueryTriggerInteraction.Ignore))
                interactable = hit.collider.GetComponentInParent<IInteractable>();
        }

        if (interactionPanel != null)
            interactionPanel.SetActive(interactable != null);

        return interactable;
    }
}
