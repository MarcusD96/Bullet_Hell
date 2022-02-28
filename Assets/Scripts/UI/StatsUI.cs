
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    public static StatsUI Instance;

    public GameObject buttonGroup;

    public Button[] hpButtons, regenButtons, damageButtons, fireRateButtons, penetrationButtons, moveSpeedButtons, projSpeedButtons;

    private Player player;
    private bool isEnabled = false;

    private void Awake() {
        if(Instance) {
            print("more than 1 statsUI, deleting");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start() {
        AddEvents(hpButtons, 0);
        AddEvents(regenButtons, 1);
        AddEvents(damageButtons, 2);
        AddEvents(fireRateButtons, 3);
        AddEvents(penetrationButtons, 4);
        AddEvents(moveSpeedButtons, 5);
        AddEvents(projSpeedButtons, 6);

        player = FindObjectOfType<Player>();

        UpdateStats();
    }

    void AddEvents(Button[] btns, int type) {
        foreach(var b in btns) {
            b.onClick.AddListener(() => IncreaseStat(type));
        }
    }

    public void SetNewPlayer(Player p) {
        player = p;
    }

    public void IncreaseStat(int type) {
        var inst = PlayerStatsManager.Instance;
        int s = 0;


        EventSystem.current.SetSelectedGameObject(null);

        switch(type) {
            case 0: { //hp
                if(inst.hpLevel >= 10)
                    break;
                inst.hpLevel++;
                s = inst.hpLevel;
                player.UpdateLevels(true);
                EventSystem.current.SetSelectedGameObject(hpButtons[s].gameObject);
                break;
            }
            case 1: { //regen
                if(inst.hpRegenLevel >= 10)
                    break;
                inst.hpRegenLevel++;
                s = inst.hpRegenLevel;
                EventSystem.current.SetSelectedGameObject(regenButtons[s].gameObject);
                break;
            }
            case 2: { //damage
                if(inst.damageLevel >= 10)
                    break;
                inst.damageLevel++;
                s = inst.damageLevel;
                EventSystem.current.SetSelectedGameObject(damageButtons[s].gameObject);
                break;
            }
            case 3: { //firerate
                if(inst.fireRateLevel >= 10)
                    break;
                inst.fireRateLevel++;
                s = inst.fireRateLevel;
                EventSystem.current.SetSelectedGameObject(fireRateButtons[s].gameObject);
                break;
            }
            case 4: { //penetration
                if(inst.penetrationLevel >= 10)
                    break;
                inst.penetrationLevel++;
                s = inst.penetrationLevel;
                EventSystem.current.SetSelectedGameObject(penetrationButtons[s].gameObject);
                break;
            }
            case 5: { //movespeed
                if(inst.moveSpeedLevel >= 10)
                    break;
                inst.moveSpeedLevel++;
                s = inst.moveSpeedLevel;
                EventSystem.current.SetSelectedGameObject(moveSpeedButtons[s].gameObject);
                break;
            }
            case 6: { //proj. speed
                if(inst.projectileSpeedLevel >= 10)
                    break;
                inst.projectileSpeedLevel++;
                s = inst.projectileSpeedLevel;
                EventSystem.current.SetSelectedGameObject(projSpeedButtons[s].gameObject);
                break;
            }
            default:
                break;
        }

        player.UpdateLevels(false);
        PlayerStatsManager.Instance.CanSubtractLevel(s);
        UpdateStats();
    }

    public void UpdateStats() {
        var inst = PlayerStatsManager.Instance;
        VerifyStatBtn(hpButtons, inst.hpLevel);
        VerifyStatBtn(regenButtons, inst.hpRegenLevel);
        VerifyStatBtn(damageButtons, inst.damageLevel);
        VerifyStatBtn(fireRateButtons, inst.fireRateLevel);
        VerifyStatBtn(penetrationButtons, inst.penetrationLevel);
        VerifyStatBtn(moveSpeedButtons, inst.moveSpeedLevel);
        VerifyStatBtn(projSpeedButtons, inst.projectileSpeedLevel);
    }

    void VerifyStatBtn(Button[] btns, int level) {
        Button b;
        for(int i = 0; i < btns.Length; i++) {
            b = btns[i];
            if(i < level)
                b.interactable = false;
            else if(i > level)
                b.gameObject.SetActive(false);
            else {
                if(PlayerStatsManager.Instance.GetLevel() < level + 1) {
                    b.gameObject.SetActive(true);
                    b.interactable = false;
                }
                else {
                    b.gameObject.SetActive(true);
                    b.interactable = true;
                }
            }
        }
    }

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        if(Input.GetButtonDown("Stat Upgrade")) {
            if(isEnabled) {
                CloseUpgrades();
                return;
            }
            OpenUpgrades();
        }
    }

    void OpenUpgrades() {
        UpdateStats();
        isEnabled = true;
        Time.timeScale = 0.2f;
        buttonGroup.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        if(FindObjectOfType<GameManager>().useController) {
            EventSystem.current.SetSelectedGameObject(hpButtons[0].gameObject); 
        }
        Cursor.visible = true;
    }

    public void CloseUpgrades() {
        isEnabled = false;
        Time.timeScale = 1f;
        buttonGroup.SetActive(false);
        Cursor.visible = false;
    }
}