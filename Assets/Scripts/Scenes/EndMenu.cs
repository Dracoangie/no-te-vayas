using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private CanvasGroup fade;
    [SerializeField, Min(0f)] private float fadeDuration = 1.2f;
    [SerializeField, Min(0f)] private float blackPause = 0.35f;
    private Player player;
    private bool ending;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        endPanel.SetActive(false);
        if (fade != null)
        {
            fade.alpha = 0f;
            fade.gameObject.SetActive(false);
        }
    }

    public void MostrarFin()
    {
        if (ending)
            return;
        ending = true;
        StartCoroutine(ShowEnding());
    }

    private IEnumerator ShowEnding()
    {
        if (InspectionController.Instance != null)
            InspectionController.Instance.Close();

        if (pauseMenu != null)
        {
            pauseMenu.Reanudar();
            pauseMenu.enabled = false;
        }
        if (player != null)
        {
            player.StopDragging();
            player.SetControlsEnabled(false);
        }
        Time.timeScale = 0f;
        endPanel.SetActive(false);

        if (fade != null)
        {
            fade.gameObject.SetActive(true);
            fade.alpha = 0f;
            fade.blocksRaycasts = true;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                fade.alpha = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / fadeDuration));
                yield return null;
            }
            fade.alpha = 1f;
            yield return new WaitForSecondsRealtime(blackPause);
        }

        endPanel.transform.SetAsLastSibling();
        endPanel.SetActive(true);
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(retryButton);
    }

    public void VolverAIntentarlo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Intro");
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
