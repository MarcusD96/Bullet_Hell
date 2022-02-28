using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

    public Transform[] fireSpawns;

    private Enemy enemyComp;
    private Player playerComp;
    private ProjectileAttack[] attacks;

    private void Awake() {
        enemyComp = GetComponent<Enemy>();
        attacks = GetComponents<ProjectileAttack>();
    }

    private void Start() {        
        playerComp = FindObjectOfType<Player>();
    }

    private void Update() {
        if(!playerComp)
            playerComp = FindObjectOfType<Player>();

        if(PauseMenu.Instance.isPaused)
            return;

        Shoot();
    }

    private void Shoot() {
        for(int i = 0; i < attacks.Length; i++) {
            attacks[i].Shoot(fireSpawns);
        }
    }
}
