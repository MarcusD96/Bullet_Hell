
using UnityEngine;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour {

    public TextMeshProUGUI hpText;
    public Transform[] fireSpawns;
    public ParticleSystem upgradeEffect;
    public GameObject deathExplosion;
    public string deathSound, hitSound;

    [HideInInspector]
    public int level, xp, penetration;

    [Header("Base Stats")]
    public int basePenetration;
    public float baseHpRegen, baseHP, baseDamage, baseFireRate, baseProjectileSpeed, baseMoveSpeed;

    [Header("Current Stats")]
    public float currentHpRegen;
    public float currentHp, maxHP, damage, fireRate, projectileSpeed, moveSpeed;

    public float nextFire = 0;

    private SpriteRenderer spriteRenderer;

    private bool isInvulnerable = false;

    public bool IsDead { get; private set; } = false;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        UpdateLevels(true, true);
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

        if(IsDead)
            return;

        if(currentHp == maxHP)
            return;

        currentHp += currentHpRegen;
        if(currentHp > maxHP)
            currentHp = maxHP;
    }

    public void TakeDamage(float damage_) {
        if(IsDead)
            return;

        if(isInvulnerable)
            return;

        currentHp -= damage_;
        if(currentHp < 1) {
            DoDie();
            return;
        }
        AudioManager.Instance.PlaySound(hitSound);

        StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash() {
        spriteRenderer.color = Color.red;
        yield return StartCoroutine(MyHelpers.WaitForTime(0.05f));
        spriteRenderer.color = Color.white;
    }

    private void DoDie() {
        IsDead = true;
        LevelStats.Level = 1;
        StartCoroutine(Die());
    }

    IEnumerator Die() {
        foreach(Transform t in transform) {
            Destroy(t.gameObject);
        }

        for(int i = 0; i < 5; i++) {
            AudioManager.Instance.PlaySound(deathSound);
            var e = Instantiate(deathExplosion, Random.insideUnitSphere + transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            e.transform.localScale = Vector2.one * Random.Range(0.25f, 1.5f);
            yield return StartCoroutine(MyHelpers.WaitForTime(0.4f));
        }

        yield return StartCoroutine(MyHelpers.WaitForTime(0.7f));
        AudioManager.Instance.PlaySound(deathSound);
        var ee = Instantiate(deathExplosion, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        ee.transform.localScale = Vector2.one * 5;

        yield return StartCoroutine(MyHelpers.WaitForTime(3f));

        if(SceneFader.Instance)
            SceneFader.Instance.FadeToScene(0);
    }

    public void UpdateLevels(bool resetHp, bool init) {
        var stats = PlayerStatsManager.Instance;
        if(resetHp) {
            if(stats.hpLevel <= 9)
                currentHp = maxHP = Mathf.CeilToInt((1 + (stats.hpLevel * 0.4f)) * baseHP);
            else
                currentHp = maxHP = Mathf.CeilToInt((1 + ((stats.hpLevel - 1) * 0.4f)) * baseHP) * 2f;
            hpText.text = currentHp.ToString();
        }

        if(stats.damageLevel <= 9)
            damage = (baseDamage * stats.damageLevel) + baseDamage;
        else
            damage = ((baseDamage * (stats.damageLevel - 1)) + baseDamage) * 2f;

        if(stats.penetrationLevel <= 9)
            penetration = stats.penetrationLevel + basePenetration;
        else
            penetration = stats.penetrationLevel - 1 + basePenetration + 5;

        if(stats.hpRegenLevel <= 9)
            currentHpRegen = baseHpRegen * Mathf.Pow(1.5f, stats.hpRegenLevel) * Time.deltaTime / 3;
        else
            currentHpRegen = (baseHpRegen * Mathf.Pow(1.5f, stats.hpRegenLevel - 1) * Time.deltaTime / 3) * 2f;

        if(stats.fireRateLevel <= 9)
            fireRate = baseFireRate * Mathf.Pow(1.1f, stats.fireRateLevel);
        else
            fireRate = (baseFireRate * Mathf.Pow(1.1f, stats.fireRateLevel - 1)) * 2f;

        if(stats.projectileSpeedLevel <= 9)
            projectileSpeed = baseProjectileSpeed * Mathf.Pow(1.1f, stats.projectileSpeedLevel);
        else
            projectileSpeed = (baseProjectileSpeed * Mathf.Pow(1.1f, stats.projectileSpeedLevel - 1)) * 1.5f;

        if(stats.moveSpeedLevel <= 9)
            moveSpeed = baseMoveSpeed * Mathf.Pow(1.1f, stats.moveSpeedLevel);
        else
            moveSpeed = (baseMoveSpeed * Mathf.Pow(1.1f, stats.moveSpeedLevel - 1)) * 2f;


        if(!init)
            upgradeEffect.Play();
    }

    public void SetInvulnerable() {
        isInvulnerable = !isInvulnerable;
    }

    public bool GetInvulnerable() {
        return isInvulnerable;
    }
}
