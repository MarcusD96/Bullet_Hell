
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour {


    #region Singleton

    public static UpgradeManager Instance;

    private void Awake() {
        if(Instance) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    #endregion

    public GameObject upgradePanel, upgradeSelections;
    public UpgradeTemplate blankUpgradePrefab;
    public Upgrade[] primaryUpgrades;
    public TextMeshProUGUI upgradeText;
    public Player CurrentGunner { get; set; }

    private Upgrade choice = null;
    private List<UpgradeTemplate> shownUpgrades = new List<UpgradeTemplate>();

    int upgradeTokens = 0;

    private void Start() {
        CurrentGunner = FindObjectOfType<Player>();
        upgradeText.gameObject.SetActive(false);
        upgradePanel.SetActive(false);
    }

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        if(upgradeTokens < 1) {
            upgradeText.gameObject.SetActive(false);
            return;
        }
        upgradeText.gameObject.SetActive(true);

        if(!Input.GetButtonDown("Gunner Upgrade"))
            return;

        if(!upgradePanel.activeSelf)
            OpenUpgrades();
        else
            CloseUpgrades();

    }

    public void AwardUpgradeToken() => upgradeTokens++;

    public void UseUpgradeToken() => upgradeTokens--;

    public void ChooseUpgrade(int choice_, Upgrade[] choices) {
        Vector2 pos = CurrentGunner.transform.position;
        Quaternion rot = CurrentGunner.transform.rotation;
        Destroy(CurrentGunner.gameObject);
        choice = choices[choice_];
        CurrentGunner = Instantiate(choice.upgradePrefab, pos, rot);
        StatsUI.Instance.SetNewPlayer(CurrentGunner);
        CloseUpgrades();
        UseUpgradeToken();
    }

    UpgradeTemplate MakeNewUpgrade(int id, Upgrade[] choices) {
        UpgradeTemplate upgrade = Instantiate(blankUpgradePrefab, upgradeSelections.transform);
        upgrade.gameObject.name = choices[id].title;
        upgrade.SetUpgrade(choices[id]);
        return upgrade;
    }

    private void OpenUpgrades() {
        StatsUI.Instance.CloseUpgrades();
        Cursor.visible = true;
        Time.timeScale = 0.2f;
        upgradePanel.gameObject.SetActive(true);
        foreach(var u in shownUpgrades) {
            Destroy(u.gameObject);
        }
        shownUpgrades.Clear();

        if(choice == null) {
            for(int i = 0; i < primaryUpgrades.Length; i++) {
                shownUpgrades.Add(MakeNewUpgrade(i, primaryUpgrades));
                SetUpgrades(i, primaryUpgrades);
            }
        }
        else {
            if(choice.childUpgrades.Length == 0) {
                CloseUpgrades();
                return;
            }
            for(int i = 0; i < choice.childUpgrades.Length; i++) {
                shownUpgrades.Add(MakeNewUpgrade(i, choice.childUpgrades));
                SetUpgrades(i, choice.childUpgrades);
            }
        }

        EventSystem.current.SetSelectedGameObject(null);
        if(FindObjectOfType<GameManager>().useController) {
            EventSystem.current.SetSelectedGameObject(shownUpgrades[0].gameObject);
        }
    }

    private void CloseUpgrades() {
        Time.timeScale = 1;
        Cursor.visible = false;
        EventSystem.current.firstSelectedGameObject = null;
        upgradePanel.SetActive(false);
        upgradeText.gameObject.SetActive(false);
    }

    void SetUpgrades(int id, Upgrade[] choices) {
        Button b = shownUpgrades[id].GetComponent<Button>();
        b.onClick.AddListener(delegate { ChooseUpgrade(id, choices); });
    }
}
