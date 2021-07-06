
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;
    Vector3 direction;

    public void Initialize(Vector2 direction_) {
        direction = direction_;
    }

    private void Update() {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
