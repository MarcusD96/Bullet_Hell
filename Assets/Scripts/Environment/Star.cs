
using UnityEngine;

public class Star : MonoBehaviour {

    private float speed = 1.5f;

    private void Update() {
        if(PauseMenu.Instance) {
            if(PauseMenu.Instance.isPaused)
                return; 
        }

        if(speed > 0) {
            transform.position -= speed * Time.deltaTime * Vector3.right;
        }
        if(transform.position.x <= -10) {
            gameObject.SetActive(false);
        }
    }
}
