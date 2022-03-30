
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
    }

    public void Settings() {
        print("make settings");
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
