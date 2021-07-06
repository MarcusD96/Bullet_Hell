
using UnityEngine;

public class Movement : MonoBehaviour {

    public Transform pivot;

    void Update() {
        RotateToMouse();
        MovementInput();
    }

    public float speed;
    void MovementInput() {
        //move up
        if(Input.GetKey(KeyCode.W))
            transform.Translate(Vector2.up * speed * Time.deltaTime);

        //move down
        if(Input.GetKey(KeyCode.S))
            transform.Translate(Vector2.down * speed * Time.deltaTime);

        //move left
        if(Input.GetKey(KeyCode.A))
            transform.Translate(Vector2.left * speed * Time.deltaTime);

        //move right
        if(Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void RotateToMouse() {
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        pivot.rotation = Quaternion.Euler(0, 0, angle);
        Debug.DrawRay(transform.position, pivot.up, Color.red, Time.deltaTime, false);
    }
}
