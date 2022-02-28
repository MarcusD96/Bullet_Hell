
using UnityEngine;

public class Movement : MonoBehaviour {

    public float joyStickDeadZone;
    public float rotSpeed = 10;
    public float screenWidth, screenHeight;
    public Transform pivot;

    private Player player;
    private TrailRenderer trailRendererComp;
    private Transform target;

    private void Awake() {
        player = GetComponent<Player>();
        trailRendererComp = GetComponent<TrailRenderer>();
        target = FindObjectOfType<TargetCursor>().target.transform;
    }

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        RotateToTarget();
        MovementInput();
    }

    public float speed;
    private void MovementInput() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);
        if(dir.magnitude > 1)
            dir.Normalize();

        transform.Translate(player.moveSpeed * Time.deltaTime * dir);

        CheckEdges();
    }

    private void RotateToTarget() {
        pivot.right = target.position - pivot.position;
    }

    private void CheckEdges() {
        Vector2 pos = transform.position;

        if(pos.x < -screenWidth) {
            pos.x = screenWidth;
            trailRendererComp.emitting = false;
            Invoke("DelayTrail", trailRendererComp.time * 2);
        }

        if(pos.x > screenWidth) {
            pos.x = -screenWidth;
            trailRendererComp.emitting = false;
            Invoke("DelayTrail", trailRendererComp.time * 2);
        }

        if(pos.y < -screenHeight) {
            pos.y = screenHeight;
            trailRendererComp.emitting = false;
            Invoke("DelayTrail", trailRendererComp.time * 2);
        }

        if(pos.y > screenHeight) {
            pos.y = -screenHeight;
            trailRendererComp.emitting = false;
            Invoke("DelayTrail", trailRendererComp.time * 2);
        }

        transform.position = pos;
    }

    void DelayTrail() {
        trailRendererComp.emitting = true;
    }
}
