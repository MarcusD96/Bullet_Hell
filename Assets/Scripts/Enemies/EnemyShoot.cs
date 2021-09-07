using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

    public float fireRate;
    public EnemyBullet weapon;

    private float nextFire = 0;
    private Enemy enemyComp;
    private Player playerComp;

    private void Awake() {
        enemyComp = GetComponent<Enemy>();
    }

    private void Start() {        
        playerComp = FindObjectOfType<Player>();
    }

    private void Update() {
        if(!playerComp)
            playerComp = FindObjectOfType<Player>();
        Shoot();
    }

    private void Shoot() {
        if(nextFire <= 0) {
            EnemyBullet b = Instantiate(weapon, transform.position, Quaternion.identity);
            b.Initialize(enemyComp.damage, 4, (playerComp.transform.position - transform.position).normalized, Quaternion.identity);
            nextFire = 1 / fireRate;
            return;
        }
        nextFire -= Time.deltaTime;
    }
}
