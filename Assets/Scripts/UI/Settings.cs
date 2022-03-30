
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public Toggle useControllerToggle;

    private void Awake() {
        useControllerToggle.isOn = GameManager.Instance.useController;
    }

    public void UseController() {
        GameManager.Instance.useController = useControllerToggle.isOn;
    }

}