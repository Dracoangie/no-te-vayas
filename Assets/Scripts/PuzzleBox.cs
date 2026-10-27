using UnityEngine;

public class PuzzleBox : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Has interactuado con el cofre.", this);
    }
}
