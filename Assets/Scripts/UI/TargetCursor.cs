
using System;
using UnityEngine;

public class TargetCursor : MonoBehaviour {

    public float horizontalBound, verticalBound;

    public GameObject target;
    public float moveSpeed;

    private Player player;

    void Awake() {
        Cursor.visible = false;
        player = FindObjectOfType<Player>();
    }

    private void LateUpdate() {
        if(player == null) {
            player = FindObjectOfType<Player>();
            return;
        }

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
        Vector2 playerPos = player.transform.position;

        Vector2 dir = mousePos - playerPos;
        dir = Vector2.ClampMagnitude(dir, (PlayerStatsManager.Instance.projectileSpeedLevel * 0.75f) + 1.5f);

        //set movement to mouse pos
        target.transform.position = Vector3.Lerp(target.transform.position, playerPos + dir, Time.unscaledDeltaTime * moveSpeed);
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
