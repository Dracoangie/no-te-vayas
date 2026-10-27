using UnityEngine;

public class InspectableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition item;

    public void Interact()
    {
        if (InspectionController.Instance == null)
        {
            Debug.LogWarning("No hay un visor de inspeccion disponible en la escena.", this);
            return;
        }

        InspectionController.Instance.Open(item);
    }
}
