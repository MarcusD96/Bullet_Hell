
using UnityEngine;

public class Star : MonoBehaviour {

    private float speed = 1.5f;

    private void Update() {
        if(speed > 0) {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if(transform.position.x <= -10) {
            gameObject.SetActive(false);
        }
    }

    public void SetStar() {
        Vector2 pos = transform.position;
        pos.x = 10;
        pos.y = Random.Range(-4.5f, 4.5f);
        transform.position = pos;
    }
}
