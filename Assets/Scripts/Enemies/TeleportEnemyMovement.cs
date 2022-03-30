
using UnityEngine;

public class TeleportEnemyMovement : EnemyMovement {

    public float vibrateAmount, warpDistance;

    Vector2 originPosition;

    private void Start() {
        originPosition = transform.position;
        InvokeRepeating(nameof(Shake), 0, 0.1f);
        Invoke(nameof(Teleport), detectionRate + Random.Range(-detectionVariation, detectionVariation));
    }

    protected override void LateUpdate() {
        if(!player)
            player = FindObjectOfType<Player>();
        if(PauseMenu.Instance.isPaused)
            return;
    }

    void Shake() {
        if(PauseMenu.Instance.isPaused)
            return;

        transform.position = originPosition + Random.insideUnitCircle * vibrateAmount;
    }

    void Teleport() {
        if(PauseMenu.Instance.isPaused)
            return;

        Vector2 playerPos;
        playerPos = player.transform.position;

        originPosition = transform.position = playerPos + (Random.insideUnitCircle * warpDistance);        
        pivot.rotation = MyHelpers.RandomZRotation;

        Invoke(nameof(Teleport), detectionRate + Random.Range(-detectionVariation, detectionVariation));
    }
}
