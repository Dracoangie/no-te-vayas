using UnityEngine;

public class InspectableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition item;

    public void Interact()
    {
        if (InspectionController.Instance == null)
        {
            Debug.LogWarning("No inspection viewer is available in the scene.", this);
            return;
        }

        InspectionController.Instance.Open(item);
    }
}
