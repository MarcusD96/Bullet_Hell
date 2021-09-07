using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulcanBullet : Weapon {

    public float critChance;

    bool hit = false;
    Enemy e;

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out e)) {
            if(!hit) {
                
                if(penetration < 2) {
                    hit = true;
                    e.GetDamaged(damage);
                    Destroy(gameObject);
                }
                else {
                    e.GetDamaged(damage);
                    penetration--;
                }
            }
        }
    }

}
