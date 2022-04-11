
using UnityEngine;

public class ClassMenu : MonoBehaviour {

    [SerializeField] GameObject firstButton;
    [SerializeField] Player[] gunners;
    [SerializeField] Upgrade[] primaryUpgrades;

    private void Start() {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void ChooseGunner(int choice_) {
        GameManager.Instance.gunner = gunners[choice_];
        GameManager.Instance.choice = primaryUpgrades[choice_];
        SceneFader.Instance.FadeToScene(2);
        AudioManager.Instance.PlaySound("Menu Click");
    }
}
