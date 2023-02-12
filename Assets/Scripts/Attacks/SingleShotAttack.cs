
using UnityEngine;

public class SingleShotAttack : ProjectileAttack {

    public override void FireProjectiles(Transform[] fireSpawns_) {
        foreach(var t in fireSpawns_) {
            EnemyProjectile b = Instantiate(projectilePrefab, t.position, Quaternion.identity);        
            b.Initialize(damage, projSpeed, MyHelpers.VaryDirection(t.right, shotVariation), null);
        }
    }
}
