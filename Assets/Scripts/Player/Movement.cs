
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float joyStickDeadZone;
    public float rotSpeed = 10;
    public float screenWidth, screenHeight;
    public Transform pivot;

    [SerializeField] ParticleSystem[] afterburners;

    private Player player;

    private float actualX = 0, actualY = 0;

    private bool autoAim = false;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        if(player.IsDead)
            return;

        RotateToTarget();
        MovementInput();
    }

    private void MovementInput() {
        if(isKnockback)
            return;

        float dt = Time.deltaTime;
        float x = Input.GetAxis("Horizontal") * dt;
        float y = Input.GetAxis("Vertical") * dt;

        if((x > 0 && actualX < 0) || (x < 0 && actualX > 0))
            x *= 3;

        if((y > 0 && actualY < 0) || (y < 0 && actualY > 0))
            y *= 3;

        actualX += x;
        actualX = Mathf.Clamp(actualX, -1f, 1f);

        actualY += y;
        actualY = Mathf.Clamp(actualY, -1f, 1f);

        if(x == 0) {
            if(actualX > 0)
                actualX -= dt / player.moveSpeed;
            else
                actualX += dt / player.moveSpeed;
        }

        if(y == 0) {
            if(actualY > 0)
                actualY -= dt / player.moveSpeed;
            else
                actualY += dt / player.moveSpeed;
        }

        Vector2 dir = new Vector2(actualX, actualY);
        if(dir.magnitude > 1)
            dir.Normalize();
        transform.Translate(player.moveSpeed * dt * dir, Space.Self);

        CheckEdges();

        DoAfterburner();
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
        if(autoAim)
            return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetRot = ((Vector3)mousePos - pivot.position).normalized;
        float dist = Vector3.Distance(pivot.right, targetRot);
        dist = Mathf.Clamp(dist, 0, 0.5f);

        pivot.right = Vector3.Lerp(pivot.right, targetRot, (4f / dist) * Time.deltaTime);
    }

    private void RotateToTarget2() {

    }

    private void CheckEdges() {
        Vector2 pos = transform.position;

        if(pos.x < -screenWidth)
            pos.x = screenWidth;

        else if(pos.x > screenWidth)
            pos.x = -screenWidth;

        else if(pos.y < -screenHeight)
            pos.y = screenHeight;

        else if(pos.y > screenHeight)
            pos.y = -screenHeight;

        transform.position = pos;
    }

    void DoAfterburner() {
        float mag = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude;

        foreach(var f in afterburners) {
            ParticleSystem.MainModule m = f.main;
            ParticleSystem.MinMaxCurve mm = m.startLifetime;

            mm.constant = mag * 0.5f;

            m.startLifetime = mm;
        }
    }

    public void ToggleAuto() {
        autoAim = !autoAim;
    }

    private void OnGUI() {
        GUI.Button(new Rect(10, 10, 150, 100), Input.GetAxis("MouseX").ToString());
    }
}
