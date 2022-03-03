
using UnityEngine;

public class PlayerBullet : Weapon {

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
