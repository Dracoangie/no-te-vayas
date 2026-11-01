using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField, TextArea] private string dialogue;
    [SerializeField, Min(0.1f)] private float duration = 4f;
    [SerializeField] private bool showOnce = true;
    [SerializeField] private bool playOnEnter = true;
    [SerializeField] private UnityEvent<string, float> onDialogue = new UnityEvent<string, float>();

    private bool hasTriggered;

    private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playOnEnter && other.GetComponentInParent<Player>() != null) PlayDialogue();
    }

    public void PlayDialogue()
    {
        if (showOnce && hasTriggered) return;
        hasTriggered = true;
        onDialogue.Invoke(dialogue, duration);
    }
}
