
using UnityEngine;

public class ProjectileAttack : MonoBehaviour {

    public float fireRate;
    public float projSpeed;
    public int damage;
    public EnemyProjectile projectilePrefab;

    private float nextFire = 0;

    public virtual bool Shoot(Transform[] fireSpawns_) {
        if(PauseMenu.Instance.isPaused)
            return false;

        nextFire -= Time.deltaTime;
        if(nextFire <= 0) {
            nextFire = 1 / fireRate;
            return true;
        }
        return false;
    }
}
