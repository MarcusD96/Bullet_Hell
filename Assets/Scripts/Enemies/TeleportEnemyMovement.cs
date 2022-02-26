using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemyMovement : EnemyMovement {

    public float vibrateAmount, warpDistance;

    Vector2 originPosition;

    private void Start() {
        originPosition = transform.position;
        InvokeRepeating("Shake", 0, 0.1f);
        InvokeRepeating("Teleport", 0, detectionRate + Random.Range(-detectionVariation, detectionVariation));
    }

    protected override void LateUpdate() {
        if(!player)
            player = FindObjectOfType<Player>();
    }

    void Shake() {
        transform.position = originPosition + Random.insideUnitCircle * vibrateAmount;
    }

    void Teleport() {
        Vector2 playerPos;

        //warp to player position
        playerPos = player.transform.position;
        transform.position = playerPos;

        //move away from player in random direction
        Vector2 warpPos = playerPos + (Vector2.up * warpDistance);
        Vector3 dir = warpPos - playerPos;
        int angle = Random.Range(0, 360);
        dir.x = (dir.x * Mathf.Cos(angle)) - (dir.y * Mathf.Sin(angle));
        dir.y = (dir.x * Mathf.Sin(angle)) + (dir.y * Mathf.Cos(angle));
        transform.position += dir;

        //update origin position
        originPosition = transform.position;
    }
}
