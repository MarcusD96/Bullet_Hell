using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float projectileMaxDistance, projectileMaxLifetime;
    public bool spreadVariation;
    [Range(0, 1)]
    public float variation;

    protected float damage;
    protected int penetration;
    protected float speed;
    protected Vector3 direction;

    private Vector3 originPosition;

    public void Initialize_Penetrate(float damage_, float speed_, int penetration_, Vector2 direction_, Quaternion rotation_) {
        damage = damage_;
        speed = speed_;
        penetration = penetration_;
        if(spreadVariation) {
            direction = VaryDirection(direction_, 0.2f);
        }
        else
            direction = direction_;
        
        UpdateRotation();
        originPosition = transform.position;
    }

    public void Initialize(int damage_, float speed_, Vector2 direction_, Quaternion rotation_) {
        damage = damage_;
        if(spreadVariation) {
            direction = VaryDirection(direction_, variation);
        }
        else
            direction = direction_;
        speed = speed_;
        
        UpdateRotation();
        originPosition = transform.position;
    }

    void Start() {
        if(projectileMaxLifetime <= 0)
            projectileMaxLifetime = 1;
        Destroy(gameObject, projectileMaxLifetime);
    }

    protected virtual void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        float dt = Time.deltaTime;
        Vector3 vel = direction * speed;
        transform.position += vel * dt;
        CheckBorder();
    }

    void FixedUpdate() {
        if(PauseMenu.Instance.isPaused)
            return;

        if(Vector2.Distance(transform.position, originPosition) > projectileMaxDistance)
            Destroy(gameObject);
    }

    private void CheckBorder() {
        if(transform.position.x > 10 ||
            transform.position.x < -10 ||
            transform.position.y > 5 ||
            transform.position.y < -5) {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        Debug.LogError("Add trigger script to: " + gameObject);
    }

    protected void UpdateRotation() {
        Vector3 rotAngle = new Vector3(0, 0, -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);
        transform.eulerAngles = rotAngle;
    }

    Vector2 VaryDirection(Vector2 direction_, float variation) {
        float rX = Random.Range(-variation, variation);
        float rY = Random.Range(-variation, variation);
        Vector2 dir = new Vector2(direction_.x + rX, direction_.y + rY);
        dir.Normalize();
        return dir;
    }
}
