using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : Enemy {

    [SerializeField] int damage;
    [SerializeField] float rotateSpeed;

    Player p;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out p)) {
            p.TakeDamage(damage);
            Die();
        }
    }

    private void Update() {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);
    }
}
