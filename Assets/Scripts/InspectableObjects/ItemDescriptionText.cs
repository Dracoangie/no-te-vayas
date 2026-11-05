using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ItemDescriptionText : MonoBehaviour
{
    // Displays the assigned item's description.
    [SerializeField] private ItemDefinition item;
    [SerializeField] private Text descriptionText;

    public void SetItem(ItemDefinition definition)
    {
        item = definition; 
        
        string description = string.Empty;
        if (item != null)
            description = item.Description;
        if (descriptionText != null && descriptionText.text != description)
            descriptionText.text = description;
    }
}
