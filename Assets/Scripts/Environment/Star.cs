
using UnityEngine;

public class Star : MonoBehaviour {

    private float speed = 1.5f;

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        if(speed > 0) {
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.position -= speed * Time.deltaTime * Vector3.right;
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

        Vector3 euler = transform.rotation.eulerAngles;
        euler.z = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(euler);

        transform.localScale *= Random.Range(0.25f, 4f);
    }
}
