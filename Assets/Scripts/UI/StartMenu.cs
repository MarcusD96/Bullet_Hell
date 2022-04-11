
using UnityEngine;

public class StartMenu : MonoBehaviour {

    [SerializeField] GameObject playButton;

    private void Start() {
        if(!GameManager.Instance.useController)
            Cursor.visible = true;
        else
            Cursor.visible = false;

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void StartGame() {
        SceneFader.Instance.FadeToScene(1);
        AudioManager.Instance.PlaySound("Menu Click");
    }

    public void Settings() {
        print("make settings");
        AudioManager.Instance.PlaySound("Menu Click");
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
