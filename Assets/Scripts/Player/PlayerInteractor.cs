using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField, Min(0.1f)] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers = Physics.DefaultRaycastLayers;
    private GameObject hint;

    private InputAction interact;
    private Player player;

    private void Awake()
    {
        interact = InputSystem.actions.FindAction("InGame/Interaction", true);
        player = GetComponent<Player>();
    }

    private void Start()
    {
        hint = GameObject.Find("Interaction Panel");
        if (hint != null)
            hint.SetActive(false);
    }

    private void Update()
    {
        IInteractable target = CheckTarget();
        if (target != null && interact.WasPressedThisFrame())
            target.Interact();

        if (!player.ControlsEnabled && hint != null)
            hint.SetActive(false);
    }

    private IInteractable CheckTarget()
    {
        IInteractable target = null;
        if (player.ControlsEnabled && Time.timeScale > 0f)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance,
                interactionLayers, QueryTriggerInteraction.Ignore))
                target = hit.collider.GetComponentInParent<IInteractable>();
        }

        if (hint != null)
            hint.SetActive(target != null);

        return target;
    }
}
