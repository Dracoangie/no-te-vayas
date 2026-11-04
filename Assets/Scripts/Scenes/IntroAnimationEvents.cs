using UnityEngine;

public class IntroAnimationEvents : MonoBehaviour
{
    [SerializeField] private IntroController introController;

    public void FinishIntro()
    {
        introController.FinishIntro();
    }
}
