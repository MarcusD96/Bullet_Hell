using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : Enemy {

    Player p;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out p)) {
            p.TakeDamage(damage);
            Die(false);
        }
    }
}
