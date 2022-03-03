
using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int baseHp, xpWorth;
    public TextMeshProUGUI hpText;
    public XPStar xpPickUp;
    public GameObject explosionPrefab;
    public float explosionScale = 1;

    private float currentHp;

    protected bool isDead = false, isBurning = false;

    private void Start() {
        currentHp = baseHp * LevelStats.Level;
        xpWorth *= Mathf.CeilToInt(LevelStats.Level / 3.0f);
        hpText.text = Mathf.Ceil(currentHp).ToString();
        ;
    }

    public void GetDamaged(float damage_) {
        currentHp -= damage_;
        hpText.text = Mathf.Ceil(currentHp).ToString();

        if(currentHp <= 0)
            Die();
    }

    public void Burn(float damage_, float time_) {
        if(!isBurning) {
            maxBurnTime = time_;
            StartCoroutine(BurnEffect(damage_));
        }
        else
            maxBurnTime = time_ + 1;
    }

    float maxBurnTime;
    IEnumerator BurnEffect(float damage_) {
        isBurning = true;
        while(maxBurnTime > 0) {
            if(PauseMenu.Instance.isPaused)
                yield return null;

            float dt = Time.deltaTime;
            maxBurnTime -= dt;
            GetDamaged(damage_ * dt);
            yield return null;
        }
        isBurning = false;
    }

    protected virtual void Die() {
        if(!isDead) {
            isDead = true;
            SpawnXP();
            EnemySpawner.Instance.RemoveEnemy(this);
            var e = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            e.transform.localScale = Vector2.one * explosionScale;
            Destroy(e, 1.5f);
            Destroy(gameObject);
        }
    }

    protected void SpawnXP() {
        var p = Instantiate(xpPickUp, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        p.InitializePickUp(xpWorth);
    }
}
