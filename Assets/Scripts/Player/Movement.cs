
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float joyStickDeadZone;
    public float rotSpeed = 10;
    public float screenWidth, screenHeight;
    public Transform pivot;

    private Player player;
    private TrailRenderer t;
    private Transform target;

    private void Awake() {
        player = GetComponent<Player>();
        t = GetComponent<TrailRenderer>();
        target = FindObjectOfType<TargetCursor>().target.transform;
    }

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        if(player.IsDead)
            return;

        t.time = 1f / player.moveSpeed * 1.5f ;

        RotateToTarget();
        MovementInput();
    }

    private void MovementInput() {
        if(isKnockback)
            return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);
        if(dir.magnitude > 1)
            dir.Normalize();

        transform.Translate(player.moveSpeed * Time.deltaTime * dir);

        CheckEdges();
    }

    bool isKnockback = false;
    public void Knockback(Vector2 direction_, float magnitude_, float time_) {
        if(isKnockback)
            return;
        StartCoroutine(DoKnockBack(direction_, magnitude_, time_));
    }

    IEnumerator DoKnockBack(Vector2 direction_, float magnituide_, float time_) {
        isKnockback = true;

        float endTime = Time.time + time_;

        while(Time.time < endTime) {
            transform.Translate(magnituide_ * Time.deltaTime * direction_);
            yield return null;
        }

        isKnockback = false;
    }

    private void RotateToTarget() {
        pivot.right = target.position - pivot.position;
    }

    private void CheckEdges() {
        Vector2 pos = transform.position;

        if(pos.x < -screenWidth) {
            pos.x = screenWidth;
            t.emitting = false;
            Invoke(nameof(DelayTrail), t.time * 2);
        }

        else if(pos.x > screenWidth) {
            pos.x = -screenWidth;
            t.emitting = false;
            Invoke(nameof(DelayTrail), t.time * 2);
        }

        else if(pos.y < -screenHeight) {
            pos.y = screenHeight;
            t.emitting = false;
            Invoke(nameof(DelayTrail), t.time * 2);
        }

        else if(pos.y > screenHeight) {
            pos.y = -screenHeight;
            t.emitting = false;
            Invoke(nameof(DelayTrail), t.time * 2);
        }

        transform.position = pos;
    }

    void DelayTrail() {
        t.emitting = true;
    }
}
