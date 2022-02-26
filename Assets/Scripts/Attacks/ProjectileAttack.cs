
using UnityEngine;

[RequireComponent(typeof(EnemyShoot))]
[RequireComponent(typeof(Enemy))]
public class ProjectileAttack : MonoBehaviour {

    public float fireRate;
    public float projSpeed;
    public EnemyProjectile projectilePrefab;

    private float nextFire = 0;

    protected Enemy enemyShip;
    protected EnemyShoot enemyShoot;

    private void Awake() {
        enemyShip = GetComponent<Enemy>();
        enemyShoot = GetComponent<EnemyShoot>();
    }

    public virtual bool Shoot(Transform[] fireSpawns_) {
        nextFire -= Time.deltaTime;
        if(nextFire <= 0) {
            nextFire = 1 / fireRate;
            return true;
        }
        return false;
    }
}
