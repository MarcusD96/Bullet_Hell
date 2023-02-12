
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

    [SerializeField] GameObject playButton;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Slider volumeSlider;
    
    private void Start() {
        if(!GameManager.Instance.useController)
            Cursor.visible = true;
        else
            Cursor.visible = false;

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(playButton);
    }

    private void Update() {
        if(settingsPanel.activeSelf == false)
            CloseSettingsKey();
    }

    public void StartGame() {
        SceneFader.Instance.FadeToScene(1);
        AudioManager.Instance.PlaySound("Menu Click");
    }

    public void OpenSettings() {
        settingsPanel.SetActive(true);
        AudioManager.Instance.PlaySound("Menu Click");
    }

    public void CloseSettingsButton() {
        settingsPanel.SetActive(false);
    }

    void CloseSettingsKey() {
        if(Input.GetKeyUp(KeyCode.Escape))
            settingsPanel.SetActive(false);
    }

    public void SetVolume() {
        AudioManager.Instance.SetMusicVolume(Mathf.Log10(volumeSlider.value) * 20);
    }

    public void Quit() {
#if UNITY_EDITOR
        AudioManager.Instance.PlaySound("Menu Click");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        AudioManager.Instance.PlaySound("Menu Click");
        Application.Quit();
#endif
    }
}
