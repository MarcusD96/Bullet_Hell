
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    #region Singleton
    public static PauseMenu Instance;

    private void Awake() {
        if(Instance) {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    #endregion

    //pause/resume
    [HideInInspector]
    public bool isPaused = false;
    bool isSettingsOpen = false;

    //settings
    public GameObject pausePanel, settingsPanel;

    private void Start() {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void Update() {
        if(Input.GetButtonDown ("Pause"))
            TogglePause();
    }

    public void TogglePause() {
        isSettingsOpen = false;
        settingsPanel.SetActive(isSettingsOpen);

        isPaused = !isPaused;
        Cursor.visible = isPaused;
        Time.timeScale = 1;
        pausePanel.SetActive(isPaused);
    }

    public void ToggleSettings() {
        isSettingsOpen = !isSettingsOpen;
        settingsPanel.SetActive(isSettingsOpen);
    }

    public void Quit() {
        UnityEditor.EditorApplication.isPlaying = false;
    }

}