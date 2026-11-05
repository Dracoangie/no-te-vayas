using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ItemDescriptionText : MonoBehaviour
{
    [SerializeField] private ItemDefinition item;
    [SerializeField] private Text descriptionText;

    public void SetItem(ItemDefinition definition)
    {
        item = definition;
        string description = item != null ? item.Description : string.Empty;
        if (descriptionText != null && descriptionText.text != description)
            descriptionText.text = description;
    }
}
