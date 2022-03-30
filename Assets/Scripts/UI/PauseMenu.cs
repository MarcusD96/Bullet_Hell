
using UnityEngine;
using UnityEngine.EventSystems;

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
    public GameObject pauseMenu, settingsMenu;

    //buttons
    public GameObject resumeButton, controllerToggle;

    private void Start() {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    private void Update() {
        if(Input.GetButtonDown("Pause"))
            TogglePause();
    }

    public void TogglePause() {
        isSettingsOpen = false;
        settingsMenu.SetActive(isSettingsOpen);
        AudioManager.Instance.PlaySound("Menu Click");

        isPaused = !isPaused;
        Cursor.visible = isPaused;
        Time.timeScale = 1;
        pauseMenu.SetActive(isPaused);

        EventSystem.current.SetSelectedGameObject(null);
        if(GameManager.Instance.useController)
            EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void ToggleSettings() {
        isSettingsOpen = !isSettingsOpen;
        settingsMenu.SetActive(isSettingsOpen);
        pauseMenu.SetActive(!isSettingsOpen);

        EventSystem.current.SetSelectedGameObject(null);
        if(GameManager.Instance.useController) {
            if(isSettingsOpen)
                EventSystem.current.SetSelectedGameObject(controllerToggle);
            else
                EventSystem.current.SetSelectedGameObject(resumeButton);
        }
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}