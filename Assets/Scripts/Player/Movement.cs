
using UnityEngine;

public class Movement : MonoBehaviour {

    public float screenWidth, screenHeight;
    public Transform pivot;

    private Player player;
    private TrailRenderer trailRendererComp;

    private void Start() {
        player = GetComponent<Player>();
        trailRendererComp = GetComponent<TrailRenderer>();
    }

    private void Update() {
        RotateToMouse();
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

    private void RotateToMouse() {
        if(Time.timeScale < 1)
            return;
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        pivot.rotation = Quaternion.Euler(0, 0, angle);
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
