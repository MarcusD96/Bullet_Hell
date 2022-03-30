
using System;
using UnityEngine;

public class TargetCursor : MonoBehaviour {

    public float horizontalBound, verticalBound;

    public GameObject target;
    public float moveSpeed;

    void Awake() {
        Cursor.visible = false;
    }

    private void LateUpdate() {
        if(PauseMenu.Instance.isPaused)
            return;

        if(!GameManager.Instance.IsStarted)
            return;

        if(!GameManager.Instance.useController) {
            MoveTargetToMouse();
        }
        else
            MoveTargetToStick();
        RestrictToBounds();
    }

    private void RestrictToBounds() {
        Vector2 pos = target.transform.position;
        pos.x = Mathf.Clamp(pos.x, -horizontalBound, horizontalBound);
        pos.y = Mathf.Clamp(pos.y, -verticalBound, verticalBound);
        target.transform.position = pos;
    }

    void MoveTargetToMouse() {
        //get input
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //set movement to mouse pos
        target.transform.position = Vector2.Lerp(target.transform.position, mousePos, Time.unscaledDeltaTime * moveSpeed);
    }

    void MoveTargetToStick() {
        //get input
        float h = Input.GetAxis("HorizontalStick_R");
        float v = Input.GetAxis("VerticalStick_R");

        Vector2 pos = new Vector2(h, v);
        if(pos.magnitude < 0.19f)
            return;
        if(pos.magnitude > 1f)
            pos.Normalize();

        target.transform.Translate(moveSpeed * Time.unscaledDeltaTime * pos);
    }
}
