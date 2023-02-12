
using UnityEngine;

public class PlayerProjectile : Weapon {

    bool hit = false;
    Enemy e;

    protected override void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);

        if(collision.TryGetComponent(out e)) {
            if(e.isDead)
                goto EndEnemy;
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
        EndEnemy:
        if(collision.TryGetComponent(out Asteroid a)) {
            if(a.isDead)
                return;
            if(!hit) {
                if(penetration < 2) {
                    hit = true;
                    a.Damage(damage);
                    Destroy(gameObject);
                }
                else {
                    a.Damage(damage);
                    penetration--;
                }
            }
        }
    }
}
