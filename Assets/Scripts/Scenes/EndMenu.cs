using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private PauseMenu pauseMenu;
    private Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        endPanel.SetActive(false);
    }

    public void MostrarFin()
    {
        if (InspectionController.Instance != null)
            InspectionController.Instance.Close();

        pauseMenu.Reanudar();
        pauseMenu.enabled = false;
        player.SetControlsEnabled(false);
        Time.timeScale = 0f;
        endPanel.SetActive(true);
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
