using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {

    #region Singleton
    public static SceneFader Instance;

    private void Awake() {
        if(Instance) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public Image image;
    public float fadeSpeed;
    public AnimationCurve fadeCurve;

    public bool isTransitioning = false;

    private void Start() {
        DontDestroyOnLoad(this);
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(int scene) {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn() {
        float t = 1.0f;

        while(t > 0.0f) {
            t -= Time.deltaTime * fadeSpeed;
            float a = fadeCurve.Evaluate(t);
            image.color = new Color(0, 0, 0, a);
            yield return null; //skip frame
        }
    }

    IEnumerator FadeOut(int scene) {
        isTransitioning = true;

        float t = 0.0f;

        while(t < 1.0f) {
            t += Time.deltaTime * fadeSpeed;
            float a = fadeCurve.Evaluate(t);
            image.color = new Color(0, 0, 0, a);
            yield return null; //skip frame
        }

        SceneManager.LoadScene(scene);
        StartCoroutine(FadeIn());
        isTransitioning = false;
    }
}
