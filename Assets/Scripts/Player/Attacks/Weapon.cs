
using UnityEngine;

public class Weapon : MonoBehaviour {

    [Header("Base Weapon")]

    public float projectileMaxDistance;
    public float projectileMaxLifetime;
    public bool spreadVariation;
    [Range(0, 1)]
    public float variation;

    protected float damage;
    protected int penetration;
    protected float speed;
    protected Vector3 direction;
    protected Player owner;

    private Vector3 originPosition;

    public virtual void InitializeWithPenetrate(float damage_, float speed_, int penetration_, Vector2 direction_, Player owner_) {
        damage = damage_;
        speed = speed_;
        penetration = penetration_;
        if(spreadVariation) {
            direction = MyHelpers.VaryDirection(direction_, variation);
        }
        else
            direction = direction_;
        owner = owner_;

        UpdateRotation();
        originPosition = transform.position;
    }

    public virtual void Initialize(int damage_, float speed_, Vector2 direction_, Player owner_) {
        damage = damage_;
        if(spreadVariation) {
            direction = MyHelpers.VaryDirection(direction_, variation);
        }
        else
            direction = direction_;
        speed = speed_;
        owner = owner_;

        UpdateRotation();
        originPosition = transform.position;
    }

    protected void Start() {
        if(projectileMaxLifetime <= 0)
            projectileMaxLifetime = 1;

        Destroy(gameObject, projectileMaxLifetime);
    }

    protected virtual void Update() {
        UpdatePathing();
    }

    protected virtual void UpdatePathing() {
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
    }

    protected void UpdateRotation() {
        Vector3 rotAngle = new Vector3(0, 0, -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);
        transform.eulerAngles = rotAngle;
    }
}