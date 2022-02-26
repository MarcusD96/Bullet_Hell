
using UnityEngine;

[System.Serializable]
public class PickUpStar : MonoBehaviour {

    public SpriteRenderer spriteR;
    public int points;
    public float moveSpeed;
    public float magnetizeSpeed;
    public float pickupDistance, magnetizeDistance;

    float rotateSpeed;
    private float baseMoveSpeed;
    Vector3 direction;
    Vector3 camExtents;

    protected Player player;

    public virtual void InitializePickUp(int points_) => points = points_;

    private void Start() {
        rotateSpeed = Random.Range(7f, 10f);
        direction = Random.insideUnitCircle.normalized;
        baseMoveSpeed = moveSpeed;
        camExtents = GetCameraExtents();
    }

    private void Update() {
        CheckBorder();
        Move();
        Magnetize();
        PickUp();
    }

    void Move() {
        spriteR.transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);
        transform.Translate(moveSpeed * Time.deltaTime * direction, Space.World);
    }

    private Vector2 GetCameraExtents() {
        return Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }

    private void CheckBorder() {
        //top/bottom
        if(transform.position.y >= camExtents.y || transform.position.y <= -camExtents.y) {
            direction.y *= -1f;
        }

        //left/right
        if(transform.position.x <= -camExtents.x || transform.position.x >= camExtents.x) {
            direction.x *= -1f;
        }
    }

    public virtual bool PickUp() {
        if(Vector2.Distance(transform.position, UpgradeManager.Instance.CurrentGunner.transform.position) <= pickupDistance)
            return true;
        return false;
    }

    void Magnetize() {
        Vector3 pos = UpgradeManager.Instance.CurrentGunner.transform.position;
        if(Vector2.Distance(transform.position, pos) <= magnetizeDistance) {
            direction = (pos - transform.position).normalized;
            moveSpeed = Mathf.Lerp(moveSpeed, magnetizeSpeed, Time.deltaTime);
        }
        else
            moveSpeed = Mathf.Lerp(moveSpeed, baseMoveSpeed, Time.deltaTime * 5f);
    }
}
