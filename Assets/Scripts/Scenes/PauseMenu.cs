using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

// Se comprueba Escape antes de que la inspeccion cierre su panel.
[DefaultExecutionOrder(-10)]
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject resumeButton;
    private Player player;
    private bool paused;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame)
            return;

        if (paused)
            Reanudar();
        else if (player.ControlsEnabled)
            Pausar();
    }

    public void Pausar()
    {
        paused = true;
        player.SetControlsEnabled(false);
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void Reanudar()
    {
        paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        player.SetControlsEnabled(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SalirDelJuego()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
