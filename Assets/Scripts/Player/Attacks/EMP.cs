using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour {

    [SerializeField] float growSpeed;
    [SerializeField] float freezeTime;

    private float startTime;
    private Enemy enemy;

    private void Start() {
        startTime = Time.time;
    }

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        transform.localScale *= 1 + (growSpeed * Time.deltaTime);
        
        if(Time.time >= startTime + 2) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out enemy)) {
            Debug.Log("enemy hit");
            enemy.GetComponent<EnemyMovement>().Freeze(freezeTime);
            enemy.GetComponent<EnemyShoot>().Freeze(freezeTime);
        }
    }

}
