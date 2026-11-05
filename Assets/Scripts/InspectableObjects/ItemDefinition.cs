using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inspection/Item")]
public class ItemDefinition : ScriptableObject
{
    // Stores an item's description and inspection settings.
    [SerializeField] private string itemName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private GameObject inspectionPrefab;
    [SerializeField] private Vector3 initialRotation;

    public string ItemName => itemName;
    public string Description => description;
    public GameObject InspectionPrefab => inspectionPrefab;
    public Vector3 InitialRotation => initialRotation;
}
