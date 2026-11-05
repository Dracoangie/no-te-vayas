using UnityEngine;

public class IntroAnimationEvents : MonoBehaviour
{
    // Notifies the intro controller when the animation finishes.
    [SerializeField] private IntroController introController;

    public void FinishIntro()
    {
        introController.FinishIntro();
    }
}
