
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy {

    [SerializeField] UpgradeStar upgradeStar;
    [SerializeField] float numSpawn;
    [SerializeField] Enemy[] babySpawns;

    bool isInvincible = false;

    bool completedPhaseOne = false;
    bool completedPhaseTwo = false;
    bool completedPhaseThree = false;

    [SerializeField] float dummySpawnTime, dummySpawnNum;
    [SerializeField] GameObject bubbleShield;

    private void Awake() {
        bubbleShield.SetActive(false);
    }

    protected override void Die() {
        if(!isDead) {
            isDead = true;
            SpawnXP();
            Instantiate(upgradeStar, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Vector2 pos = transform.position;
            for(int i = 0; i < numSpawn; i++) {
                EnemySpawner.Instance.GetSpawnedEnemies().Add(Instantiate(babySpawns[0], transform.position + (Vector3) Random.insideUnitCircle * 2, Quaternion.identity));
            }
            EnemySpawner.Instance.RemoveEnemy(this);
            var e = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            e.transform.localScale = Vector2.one * explosionScale;
            AudioManager.Instance.PlaySound(deathSound);
            Destroy(e, 1.5f);
            Destroy(gameObject);
        }
    }

    private void Update() {
        if(!completedPhaseOne)
            if(currentHp <= baseHp * LevelStats.Level * 0.75f)
                StartCoroutine(DoInvincibility(1));
        if(!completedPhaseTwo)
            if(currentHp <= baseHp * LevelStats.Level * 0.5f)
                StartCoroutine(DoInvincibility(2));
        if(!completedPhaseThree)
            if(currentHp <= baseHp * LevelStats.Level * 0.25f)
                StartCoroutine(DoInvincibility(3));
    }

    IEnumerator SpawnDummies() {
        //spawn smaller enemies while invincible, becomes uninvincible if all enemies are defeated
        for(int i = 0; i < dummySpawnNum; i++) {

            var b = Instantiate(babySpawns[Random.Range(0, babySpawns.Length)], transform.position + (Vector3) (Random.insideUnitCircle * 2), Quaternion.identity);
            EnemySpawner.Instance.GetSpawnedEnemies().Add(b);

            yield return StartCoroutine(MyHelpers.WaitForTime(dummySpawnTime/dummySpawnNum));
        }

        while(EnemySpawner.Instance.GetSpawnedEnemies().Count > 1)
            yield return null;
    }

    public override void GetDamaged(float damage_) {
        if(!isInvincible)
            base.GetDamaged(damage_);
    }

    IEnumerator DoInvincibility(int stageNum) {
        isInvincible = true;
        GetComponent<Collider2D>().enabled = false;
        bubbleShield.SetActive(true);

        foreach(var c in GetComponents<ProjectileAttack>()) {
            c.ResetFire(1000f);
        }

        switch(stageNum) {
            case 1:
                completedPhaseOne = true;
                break;

            case 2:
                completedPhaseTwo = true;
                break;

            case 3:
                completedPhaseThree = true;
                break;

            default:
                break;
        }

        yield return StartCoroutine(SpawnDummies());

        foreach(var c in GetComponents<ProjectileAttack>()) {
            c.SkipFire();
        }

        GetComponent<Collider2D>().enabled = true;
        isInvincible = false;
        bubbleShield.SetActive(false);
    }

    public bool IsInvincible() {
        return isInvincible;
    }
}
