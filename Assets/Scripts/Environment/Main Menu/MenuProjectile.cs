using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuProjectile : MonoBehaviour {

    [SerializeField] float moveSpeed;

    private void Start() {
        Destroy(gameObject, 3.0f);
    }

    private void Update() {
        transform.position += moveSpeed * Time.deltaTime * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out MenuEnemy e)) {
            e.Hit();
            Destroy(gameObject);
        }
    }
}
