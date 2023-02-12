
using System.Collections;
using UnityEngine;

public class TeleportEnemyMovement : EnemyMovement {

    public float vibrateAmount, warpDistance;
    [SerializeField] TPShadowClone teleportShadow;

    Vector2 originPosition;

    private void Start() {
        originPosition = transform.position;
        InvokeRepeating(nameof(Shake), 0, 0.1f);
        StartCoroutine(TeleportToLocation());
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

        if(isFrozen == true)
            return;

        transform.position = originPosition + Random.insideUnitCircle * vibrateAmount;
        pivot.rotation = MyHelpers.RandomZRotation;
    }

    IEnumerator TeleportToLocation() {
        //delay for shoot
        yield return MyHelpers.WaitForTime(detectionRate + Random.Range(-detectionVariation, detectionVariation));

        if(isFrozen ==  true) {
            StartCoroutine(TeleportToLocation());
            yield break;
        }

        //find tpPos
        Vector3 teleportLocation = player.transform.position + (Vector3) (Random.insideUnitCircle * warpDistance);
        Vector3 startLocation = transform.position;

        //spawn shadow at tpPos
        var s = Instantiate(teleportShadow, transform.position, MyHelpers.RandomZRotation);
        s.Initialize(gameObject);

        //move to tpPos
        float endTime = Random.Range(1, 3);
        float t = 0;
        while(t < endTime) {
            if(PauseMenu.Instance)
                if(PauseMenu.Instance.isPaused) {
                    yield return null;
                    continue;
                }
            t += Time.deltaTime;

            s.transform.position = Vector3.Lerp(startLocation, teleportLocation, t / endTime);

            yield return null;
        }

        //set pos to tpPos
        originPosition = transform.position = teleportLocation;
        pivot.rotation = MyHelpers.RandomZRotation;

        s.Destroy();

        //start next tp
        StartCoroutine(TeleportToLocation());
    }
}
