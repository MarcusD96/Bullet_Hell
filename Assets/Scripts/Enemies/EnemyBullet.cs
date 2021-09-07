
using UnityEngine;

public class EnemyBullet : Weapon {

    Player player;
    protected override void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out player)) {
            player.TakeDamage(damage);
            Destroy(gameObject); 
        }
    }
}
