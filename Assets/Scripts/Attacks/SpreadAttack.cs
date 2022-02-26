using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadAttack : ProjectileAttack {

    public override bool Shoot(Transform[] fireSpawns_) {
        if(base.Shoot(fireSpawns_)) {
            foreach(var f in fireSpawns_) {
                var p = Instantiate(projectilePrefab, f.position, f.rotation);
                p.Initialize(enemyShip.damage, enemyShoot.projSpeed ,f.right, f.rotation);
            }
            return true;
        }
        return false;
    }

}
