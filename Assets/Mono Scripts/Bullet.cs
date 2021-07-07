
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;
    public Transform sprite;

    int damage;
    Vector3 direction;

    public void Initialize(int damage_, Vector2 direction_, Quaternion rotation_) {
        damage = damage_;
        direction = direction_;
        sprite.rotation = Quaternion.Euler(rotation_.eulerAngles + (Vector3.forward * 45));
    }

    private void Update() {
        transform.Translate(direction * speed * Time.deltaTime);
        CheckBorder();
    }

    void CheckBorder() {
        if(transform.position.x > 10 ||
            transform.position.x < -10 ||
            transform.position.y > 5 ||
            transform.position.y < -5) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        collision.GetComponent<Enemy>().GetDamaged(damage);
        Destroy(gameObject);
    }
}
