using UnityEngine;
using UnityEngine.Events;

public class InspectableObject : MonoBehaviour, IInteractable
{
    // Opens an item in the inspection viewer.
    [SerializeField] private ItemDefinition item;
    [SerializeField] private UnityEvent onInteracted = new UnityEvent();

    public void Interact()
    {
        onInteracted.Invoke();

        if (InspectionController.Instance == null)
        {
            Debug.LogWarning("No inspection viewer is available in the scene.", this);
            return;
        }

        InspectionController.Instance.Open(item);
    }
}
