
using UnityEngine;

public class SingleShotAttack : ProjectileAttack {

    public override void FireProjectiles(Transform[] fireSpawns_) {
        EnemyProjectile b = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        b.Initialize(damage, projSpeed, fireSpawns_[0].right);
    }
}
