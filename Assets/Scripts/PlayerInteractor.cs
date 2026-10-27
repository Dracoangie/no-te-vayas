using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField, Min(0.1f)] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers = Physics.DefaultRaycastLayers;

    private InputAction interactionAction;

    private void Awake()
    {
        interactionAction = InputSystem.actions.FindAction("InGame/Interaction", true);

        if (playerCamera == null)
        {
            Debug.LogError("Asigna la camara del jugador en PlayerInteractor.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (!interactionAction.WasPressedThisFrame())
            return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance,
            interactionLayers, QueryTriggerInteraction.Ignore))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            interactable?.Interact();
        }
    }
}
