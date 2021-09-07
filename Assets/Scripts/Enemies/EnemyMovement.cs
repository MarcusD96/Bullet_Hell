using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float speed;
    public float detectionRate, detectionVariation;

    private Vector2 direction;

    protected Player player;

    private void Awake() {
        float rateMin = detectionRate - detectionVariation;
        if(rateMin < 0)
            rateMin = 0.1f;
        float rateMax = detectionRate + detectionVariation;
        if(speed <= 0)
            return;
        InvokeRepeating("UpdatePlayerPosition", 0.1f, Random.Range(rateMin, rateMax));
    }

    private void Start() {
        player = FindObjectOfType<Player>();
    }

    protected virtual void LateUpdate() {
        if(!player)
            player = FindObjectOfType<Player>();

        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void UpdatePlayerPosition() {
        direction = (player.transform.position - transform.position).normalized;
    }
}
