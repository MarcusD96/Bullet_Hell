
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {

    public GameObject upgradePanel, upgradeSelections;
    public UpgradeTemplate blankUpgradePrefab;
    public Upgrade[] primaryUpgrades;
    public TextMeshProUGUI upgradeText;

    private Upgrade choice = null;
    private List<UpgradeTemplate> shownUpgrades = new List<UpgradeTemplate>();
    private Player currentGunner;
    private bool finalUpgrade = false;

    int lowestCost;
    private void Start() {
        currentGunner = FindObjectOfType<Player>();
        lowestCost = int.MaxValue;
        FindLowestCost(primaryUpgrades);
        upgradeText.gameObject.SetActive(false);
        upgradePanel.SetActive(false);
    }

    private void Update() {
        if(PlayerStatsManager.Instance.GetLevel() >= lowestCost && !finalUpgrade) {
            upgradeText.gameObject.SetActive(true);
            if(Input.GetKeyDown(KeyCode.E))
                OpenUpgrades();
        }
        else if(upgradeText.gameObject.activeSelf)
            upgradeText.gameObject.SetActive(false);
    }

    void FindLowestCost(Upgrade[] upgrades) {
        lowestCost = int.MaxValue;
        foreach(var u in upgrades) {
            if(u.cost < lowestCost)
                lowestCost = u.cost;
        }
    }

    public void ChooseUpgrade(int choice_, Upgrade[] choices) {
        Vector2 pos = currentGunner.transform.position;
        Quaternion rot = currentGunner.transform.rotation;
        Destroy(currentGunner.gameObject);
        if(choice != null)
            finalUpgrade = true;
        choice = choices[choice_];
        currentGunner = Instantiate(choice.upgradePrefab, pos, rot);
        StatsUI.Instance.SetNewPlayer(currentGunner);
        CloseUpgrades();
        PlayerStatsManager.Instance.CanSubtractLevel(choice.cost);
        FindLowestCost(choice.childUpgrades);
    }

    UpgradeTemplate MakeNewUpgrade(int id, Upgrade[] choices) {
        UpgradeTemplate upgrade = Instantiate(blankUpgradePrefab, upgradeSelections.transform);
        upgrade.gameObject.name = choices[id].title;
        upgrade.SetUpgrade(choices[id]);
        return upgrade;
    }

    private void OpenUpgrades() {

        Time.timeScale = 0;
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
    }

    private void CloseUpgrades() {
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
        upgradeText.gameObject.SetActive(false);
    }

    void SetUpgrades(int id, Upgrade[] choices) {
        Button b = shownUpgrades[id].GetComponent<Button>();
        b.onClick.AddListener(delegate { ChooseUpgrade(id, choices); });
        if(PlayerStatsManager.Instance.GetLevel() < primaryUpgrades[id].cost)
            b.interactable = false;
    }
}
