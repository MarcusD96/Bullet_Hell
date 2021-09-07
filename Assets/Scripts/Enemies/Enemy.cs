
using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int baseHp, xpWorth, damage;
    public TextMeshProUGUI hpText;

    private float currentHp;

    protected bool isDead = false, isBurning = false;

    private void Start() {
        currentHp = baseHp * LevelStats.Level;
        xpWorth *= Mathf.CeilToInt(LevelStats.Level / 3.0f);
        hpText.text = currentHp.ToString();
    }

    public void GetDamaged(float damage_) {
        currentHp -= damage_;
        hpText.text = currentHp.ToString("F0");

        if(currentHp <= 0)
            Die(true);
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
            float dt = Time.deltaTime;
            maxBurnTime -= dt;
            GetDamaged(damage_ * dt);
            yield return null;
        }
        isBurning = false;
    }

    protected virtual void Die(bool awardPoints) {
        if(!isDead) {
            isDead = true;
            if(awardPoints) {
                PlayerStatsManager.Instance.AddXP(xpWorth); 
            }
            EnemySpawner.Instance.RemoveEnemy(this);
            Destroy(gameObject); 
        }
    }
}
