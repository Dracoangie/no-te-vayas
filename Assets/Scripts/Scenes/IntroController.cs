using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class IntroController : MonoBehaviour
{
    // Handles the intro sequence and loads the game scene.
    [SerializeField] private string nextScene = "NightHouse";
    [SerializeField] private CanvasGroup content;
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private Button enterButton;
    [SerializeField] private Animator doorwayAnimator;
    [SerializeField, Min(0.01f)] private float revealDuration = 1.4f;

    private bool ready;
    private bool transitioning;
    private bool animationFinished;

    private IEnumerator Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        enterButton.interactable = false;
        content.alpha = 0f;
        fade.alpha = 1f;
        fade.blocksRaycasts = true;

        yield return Fade(1f, 0f, revealDuration, true);

        fade.blocksRaycasts = false;
        enterButton.interactable = true;
        ready = true;
    }

    private void Update()
    {
        if (!ready || transitioning)
            return;

        Keyboard keyboard = Keyboard.current;
        Gamepad gamepad = Gamepad.current;
        bool keyboardStart = keyboard != null &&
            (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame ||
             keyboard.spaceKey.wasPressedThisFrame);
        bool gamepadStart = gamepad != null &&
            (gamepad.startButton.wasPressedThisFrame || gamepad.buttonSouth.wasPressedThisFrame);

        if (keyboardStart || gamepadStart)
            EnterHouse();
    }

    public void SalirDelJuego()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void EnterHouse()
    {
        if (!ready || transitioning)
            return;

        transitioning = true;
        enterButton.interactable = false;
        content.blocksRaycasts = false;
        fade.blocksRaycasts = true;

        CameraSway cameraSway = doorwayAnimator.GetComponentInChildren<CameraSway>();
        if (cameraSway != null)
            cameraSway.enabled = false;

        doorwayAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        doorwayAnimator.SetTrigger("StartGame");
        DontDestroyOnLoad(gameObject);
        StartCoroutine(contentFade());
        StartCoroutine(LoadHouse());
    }

    private IEnumerator contentFade()
    {
        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.5f));
            content.alpha = 1f - progress;
            yield return null;
        }
        content.alpha = 0f;
        content.gameObject.SetActive(false);
    }

    private IEnumerator LoadHouse()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
        loading.allowSceneActivation = false;

        while (!animationFinished)
        {
            AnimatorStateInfo state = doorwayAnimator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("IntroAnim"))
                fade.alpha = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(state.normalizedTime));
            yield return null;
        }

        while (loading.progress < 0.9f)
            yield return null;

        loading.allowSceneActivation = true;
        while (!loading.isDone)
            yield return null;

        yield return Fade(1f, 0f, revealDuration);
        Destroy(gameObject);
    }

    public void FinishIntro()
    {
        if (!transitioning)
            return;

        fade.alpha = 1f;
        animationFinished = true;
    }

    private IEnumerator Fade(float from, float to, float duration, bool revealContent = false)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
            fade.alpha = Mathf.Lerp(from, to, progress);
            if (revealContent)
                content.alpha = progress;
            yield return null;
        }

        fade.alpha = to;
        if (revealContent)
            content.alpha = 1f;
    }
}
