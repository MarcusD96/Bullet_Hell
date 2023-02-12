using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {


    public MoveMode moveMode;
    public float speed;
    public float maxPivotDegrees;
    public float detectionRate, detectionVariation;
    public Transform pivot;

    protected Player player;
    protected Vector2 playerPos;
    protected bool isFrozen = false;

    private Enemy enemyComp;
    private Vector2 moveDirection, aimDirection;
    private float minRate, maxRate;

    private void Start() {
        player = FindObjectOfType<Player>();
        enemyComp = GetComponent<Enemy>();

        minRate = detectionRate - detectionVariation;
        if(minRate < 0)
            minRate = 0.1f;
        maxRate = detectionRate + detectionVariation;

        if(speed <= 0)
            return;

        UpdateMoveDirection();
    }

    protected virtual void LateUpdate() {
        if(!player) {
            player = FindObjectOfType<Player>();
            return;
        }

        if(PauseMenu.Instance.isPaused)
            return;

        if(isFrozen == true)
            return;

        UpdateAimDirection();
        if(pivot) {
            pivot.right = aimDirection;
        }

        transform.Translate(speed * Time.deltaTime * moveDirection);
    }

    private void UpdateMoveDirection() {
        switch(moveMode) {
            case MoveMode.Around:
                Vector2 target = player.transform.position + (Random.onUnitSphere * 3f);
                moveDirection = (target - (Vector2) transform.position).normalized;
                break;
            case MoveMode.Middle:
                moveDirection = (player.transform.position - transform.position).normalized;
                break;
            case MoveMode.Random:
                moveDirection = (MyHelpers.GetRandomPointOnScreen(enemyComp.size) - transform.position).normalized;
                break;
            default:
                break;
        }

        Invoke(nameof(UpdateMoveDirection), Random.Range(minRate, maxRate));
    }

    private void UpdateAimDirection() {
        Vector2 targetDirection = (player.transform.position - transform.position).normalized;
        aimDirection = Vector3.RotateTowards(aimDirection.normalized, targetDirection, maxPivotDegrees * Mathf.Deg2Rad * Time.deltaTime, float.MaxValue);
    }

    void UpdatePlayerPosition() => playerPos = player.transform.position;

    public void Freeze(float time) {
        StartCoroutine(FreezeTime(time));
    }

    IEnumerator FreezeTime(float time) {
        isFrozen = true;
        yield return MyHelpers.WaitForTime(time);
        isFrozen = false;
    }
}

public enum MoveMode {
    Around,
    Middle,
    Random
}
