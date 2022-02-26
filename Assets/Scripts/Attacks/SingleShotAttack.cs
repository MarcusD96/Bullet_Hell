
using UnityEngine;

public class SingleShotAttack : ProjectileAttack {



    public override bool Shoot(Transform[] fireSpawns_) {
        if(base.Shoot(fireSpawns_)) {
            EnemyProjectile b = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            b.Initialize(enemyShip.damage, projSpeed, fireSpawns_[0].right, Quaternion.identity);
            return true;
        }
        return false;
    }

}
