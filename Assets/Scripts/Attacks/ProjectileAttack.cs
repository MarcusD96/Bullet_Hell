
using System.Collections;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour {

    public float fireRate;
    public int burstNum;
    public float projSpeed;
    public int damage;
    public EnemyProjectile projectilePrefab;

    private float nextFire = 0;
    protected Coroutine burst;

    public void Shoot(Transform[] fireSpawns_) {
        if(PauseMenu.Instance.isPaused)
            return;

        nextFire -= Time.deltaTime;
        if(nextFire <= 0) {
            nextFire = 1 / fireRate;
            StartCoroutine(FireBurstShoot(fireSpawns_));
            return;
        }
    }

    public virtual void FireProjectiles(Transform[] fireSpawns_) {
        print("TODO: implement override FireProjectiles");
    }

    public IEnumerator FireBurstShoot(Transform[] fireSpawns_) {
        float shotTime = (1 / fireRate) / (burstNum * 4);
        for(int i = 0; i < burstNum; i++) {
            FireProjectiles(fireSpawns_);
            yield return StartCoroutine(MyHelpers.WaitForTime(shotTime));
        }
        burst = null;
    }
}