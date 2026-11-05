using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class MapEventTrigger : MonoBehaviour
{
    // Runs configured events when the player enters a trigger.
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private UnityEvent onTriggered = new UnityEvent();

    private bool hasTriggered;

    private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>() != null)
            TriggerEvent();
    }

    public void TriggerEvent()
    {
        if (triggerOnce && hasTriggered) return;

        hasTriggered = true;
        onTriggered.Invoke();
    }
}
