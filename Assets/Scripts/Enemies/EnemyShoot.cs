using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

    public Transform[] fireSpawns;

    private Player playerComp;
    private ProjectileAttack[] attacks;
    private bool isFrozen = false; 

    private void Awake() {
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

        if(isFrozen == true)
            return;

        Shoot();
    }

    private void Shoot() {
        for(int i = 0; i < attacks.Length; i++) {
            attacks[i].Shoot(fireSpawns);
        }
    }

    public void Freeze(float time) {
        StartCoroutine(FreezeTime(time));
    }

    IEnumerator FreezeTime(float time) {
        isFrozen = true;
        yield return MyHelpers.WaitForTime(time);
        isFrozen = false;
    }
}
