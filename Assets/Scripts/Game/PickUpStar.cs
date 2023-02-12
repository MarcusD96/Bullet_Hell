
using UnityEngine;

[System.Serializable]
public class PickUpStar : MonoBehaviour {



    public SpriteRenderer spriteR;
    public float radius;

    public string pickUpSound;

    public int points;
    public float moveSpeed;
    public float magnetizeSpeed;
    public float pickupDistance, magnetizeDistance;

    float rotateSpeed;
    private float baseMoveSpeed;
    Vector3 direction;
    Vector3 camExtents;

    protected Player player;

    private void Awake() {
        player = FindObjectOfType<Player>();        
    }

    public void InitializePickUp(int points_) {
        if(points <= 0) {
            points = 1;
            print("less than 0 points");
            return;
        }
        points = points_;
    }

    private void Start() {
        rotateSpeed = Random.Range(7f, 10f);
        direction = (player.transform.position - transform.position).normalized;
        baseMoveSpeed = moveSpeed;
        camExtents = GetCameraExtents();
    }

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

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
        if(transform.position.y + radius >= camExtents.y || transform.position.y - radius <= -camExtents.y) {
            direction.y *= -1f;
        }

        //left/right
        if(transform.position.x - radius <= -camExtents.x || transform.position.x + radius >= camExtents.x) {
            direction.x *= -1f;
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -camExtents.x + radius, camExtents.x - radius);
        pos.y = Mathf.Clamp(pos.y, -camExtents.y + radius, camExtents.y - radius);
        transform.position = pos;
    }

    public virtual bool PickUp() {
        if(Vector2.Distance(transform.position, UpgradeManager.Instance.CurrentGunner.transform.position) <= pickupDistance) {
            AudioManager.Instance.PlaySound(pickUpSound);
            return true;
        }
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

    [ExecuteInEditMode]
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
