using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Stats")]
    public int health;

    Shoot shootComp;
    Movement moveComp;

    private void Awake() {
        shootComp = GetComponent<Shoot>();
        moveComp = GetComponent<Movement>();
    }

    public void TakeDamage(int damage_) {
        health -= damage_;

        if(health <= 0)
            Die();
    }

    void Die() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
