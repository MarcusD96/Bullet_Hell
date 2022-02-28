
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour {

    public static PlayerStatsManager Instance;

    public int damageLevel, fireRateLevel, penetrationLevel, projectileSpeedLevel, moveSpeedLevel, hpLevel, hpRegenLevel;
    public TextMeshProUGUI levelText, xpText;
    public Image xpBar;

    int level, currentXP, targetXP;

    private void Awake() {
        if(Instance) {
            print("more than 1 stats manager...deleting new one");
            Destroy(this);
            return;
        }
        Instance = this;

        currentXP = level = 0;
        targetXP = 2;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q))
            level += 10;
    }

    private void LateUpdate() {
        if(PauseMenu.Instance.isPaused)
            return;

        levelText.text = level.ToString();
        xpText.text = currentXP.ToString() + "/" + targetXP.ToString();
        xpBar.fillAmount = (float) currentXP / (float) targetXP;
        CheckLevelUp();
    }

    public void CheckLevelUp() {
        targetXP = (level * 2) + 2;
        if(currentXP == targetXP) {
            currentXP = 0;
            level++;
        }
        else if(currentXP > targetXP) {
            currentXP -= targetXP;
            level++;
            CheckLevelUp();
        }
    }

    public int GetLevel() {
        return level;
    }

    public void AddXP(int num) {
        currentXP += num;
        StatsUI.Instance.UpdateStats();
    }

    public void CanSubtractLevel(int num) {
        level -= num;
    }
}
