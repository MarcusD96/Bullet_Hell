
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public static StatsUI Instance;

    public Button[] hpButtons, regenButtons, damageButtons, fireRateButtons, penetrationButtons, moveSpeedButtons, projSpeedButtons;

    private Player player;
    private Animator animator;

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
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdateStats", 0, 0.1f);
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

        switch(type) {
            case 0: { //hp
                if(inst.hpLevel >= 10)
                    break;
                inst.hpLevel++;
                s = inst.hpLevel;
                player.UpdateLevels(true);
                break;
            }
            case 1: { //regen
                if(inst.hpRegenLevel >= 10)
                    break;
                inst.hpRegenLevel++;
                s = inst.hpRegenLevel;
                break;
            }
            case 2: { //damage
                if(inst.damageLevel >= 10)
                    break;
                inst.damageLevel++;
                s = inst.damageLevel;
                break;
            }
            case 3: { //firerate
                if(inst.fireRateLevel >= 10)
                    break;
                inst.fireRateLevel++;
                s = inst.fireRateLevel;
                break;
            }
            case 4: { //penetration
                if(inst.penetrationLevel >= 10)
                    break;
                inst.penetrationLevel++;
                s = inst.penetrationLevel;
                break;
            }
            case 5: { //movespeed
                if(inst.moveSpeedLevel >= 10)
                    break;
                inst.moveSpeedLevel++;
                s = inst.moveSpeedLevel;
                break;
            }
            case 6: { //proj. speed
                if(inst.projectileSpeedLevel >= 10)
                    break;
                inst.projectileSpeedLevel++;
                s = inst.projectileSpeedLevel;
                break;
            }
            default:
                break;
        }

        player.UpdateLevels(false);
        PlayerStatsManager.Instance.CanSubtractLevel(s);
    }

    void UpdateStats() {
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

    public void OnPointerEnter(PointerEventData eventData) {
        animator.SetBool("onStats", true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        animator.SetBool("onStats", false);
    }
}