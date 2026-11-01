using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup dialogueGroup;
    [SerializeField] private Text dialogueLabel;
    [SerializeField, Min(0.1f)] private float fadeSpeed = 5f;

    private readonly Queue<(string text, float duration)> dialogues = new Queue<(string, float)>();
    private bool displaying;

    private void Awake()
    {
        dialogueGroup.alpha = 0f;
    }

    public void ShowDialogue(string text, float duration)
    {
        if (!isActiveAndEnabled || string.IsNullOrWhiteSpace(text)) return;
        dialogues.Enqueue((text, duration));
        if (!displaying) StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        displaying = true;
        while (dialogues.Count > 0)
        {
            var dialogue = dialogues.Dequeue();
            dialogueLabel.text = dialogue.text;
            yield return Fade(1f);
            yield return new WaitForSecondsRealtime(dialogue.duration);
            yield return Fade(0f);
        }
        dialogueLabel.text = string.Empty;
        displaying = false;
    }

    private IEnumerator Fade(float alpha)
    {
        while (dialogueGroup.alpha != alpha)
        {
            dialogueGroup.alpha = Mathf.MoveTowards(dialogueGroup.alpha, alpha, Time.unscaledDeltaTime * fadeSpeed);
            yield return null;
        }
    }

    public void ClearDialogue()
    {
        StopAllCoroutines();
        dialogues.Clear();
        displaying = false;
        if (dialogueGroup != null) dialogueGroup.alpha = 0f;
        if (dialogueLabel != null) dialogueLabel.text = string.Empty;
    }

    private void OnDisable()
    {
        ClearDialogue();
    }
}
