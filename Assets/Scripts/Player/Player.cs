
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour {

    public TextMeshProUGUI hpText;
    public Transform[] fireSpawns;
    public ParticleSystem upgradeEffect;

    [HideInInspector]
    public int level, xp, penetration;

    [Header("Base Stats")]
    public int basePenetration;
    public float baseHP, baseDamage, baseFireRate, baseProjectileSpeed, baseMoveSpeed;

    [Header("Current Stats")]
    public float hpRegen;
    public float currentHp, maxHP, damage, fireRate, projectileSpeed, moveSpeed;

    public float nextFire = 0;

    private void Start() {
        UpdateLevels(true);
        nextFire = 0;
    }

    private void LateUpdate() {
        if(PauseMenu.Instance.isPaused)
            return;

        RegenHP();
    }

    private void RegenHP() {
        currentHp = Mathf.Clamp(currentHp, 0, currentHp);
        hpText.text = Mathf.FloorToInt(currentHp).ToString();

        if(currentHp == maxHP)
            return;
        currentHp += hpRegen;
        if(currentHp > maxHP)
            currentHp = maxHP;
    }

    public void TakeDamage(float damage_) {
        currentHp -= damage_;


        if(currentHp < 1)
            Die();
    }

    private void Die() {
        print("TODO: implement new game screen");
        LevelStats.Level = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void UpdateLevels(bool resetHp) {
        var stats = PlayerStatsManager.Instance;
        if(resetHp) {
            currentHp = maxHP = Mathf.CeilToInt((1 + (stats.hpLevel * 0.25f)) * baseHP);
            hpText.text = currentHp.ToString();
        }
        hpRegen = stats.hpRegenLevel * Time.deltaTime / 3;
        damage = Mathf.CeilToInt(baseDamage * stats.damageLevel) + baseDamage;
        fireRate = (1 + (stats.fireRateLevel * 0.5f)) * baseFireRate;
        penetration = stats.penetrationLevel + basePenetration;
        projectileSpeed = (1 + (stats.projectileSpeedLevel * 0.5f)) * baseProjectileSpeed;
        moveSpeed = (1 + (stats.moveSpeedLevel * 0.5f)) * baseMoveSpeed;

        upgradeEffect.Play();
    }
}
