using System.Collections;
using UnityEngine;

public class SpreadAttack : ProjectileAttack {

    public override void FireProjectiles(Transform[] fireSpawns_) {
        foreach(var f in fireSpawns_) {
            var p = Instantiate(projectilePrefab, f.position, f.rotation);
            p.Initialize(damage, projSpeed, f.right);
        }
    }
}
