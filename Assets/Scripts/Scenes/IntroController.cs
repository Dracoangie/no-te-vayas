using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class IntroController : MonoBehaviour
{
    [SerializeField] private string nextScene = "NightHouse";
    [SerializeField] private CanvasGroup content;
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private Button enterButton;
    [SerializeField] private Text loadingLabel;
    [SerializeField, Min(0.01f)] private float revealDuration = 1.4f;
    [SerializeField, Min(0.01f)] private float transitionDuration = 0.9f;

    private bool ready;
    private bool transitioning;

    private IEnumerator Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        enterButton.interactable = false;
        content.alpha = 0f;
        fade.alpha = 1f;
        fade.blocksRaycasts = true;
        loadingLabel.gameObject.SetActive(false);

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

    public void EnterHouse()
    {
        if (!ready || transitioning)
            return;

        if (!Application.CanStreamedLevelBeLoaded(nextScene))
        {
            Debug.LogError($"Add the {nextScene} scene to the Build Profiles scene list.", this);
            return;
        }

        transitioning = true;
        enterButton.interactable = false;
        content.blocksRaycasts = false;
        fade.blocksRaycasts = true;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(LoadHouse());
    }

    private IEnumerator LoadHouse()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
        loading.allowSceneActivation = false;

        yield return Fade(0f, 1f, transitionDuration);
        content.gameObject.SetActive(false);
        loadingLabel.gameObject.SetActive(true);

        while (loading.progress < 0.9f)
            yield return null;

        loading.allowSceneActivation = true;
        while (!loading.isDone)
            yield return null;

        loadingLabel.gameObject.SetActive(false);
        yield return Fade(1f, 0f, transitionDuration);
        Destroy(gameObject);
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
