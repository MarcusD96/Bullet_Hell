
using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    [HideInInspector]
    public float startHP;

    public Asteroid childPrefab;
    public SpriteRenderer spriteRenderer;

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] XPStar XPStar;
    [SerializeField] bool isChild;
    float currentHP;
    float moveSpeed, rotateSpeed;
    float radius;
    Vector3 direction;
    Asteroid[] childrenAstroids;
    bool isDead = false;

    private void Start() {
        direction = transform.right.normalized;
        camExtents = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        radius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;

        if(isChild)
            return;

        currentHP = startHP;
        transform.localScale *= Random.Range(0.5f, 1.5f);
        moveSpeed = Random.Range(0.5f, 1.25f);
        rotateSpeed = Random.Range(2f, 15f);
    }

    private void Update() {
        if(PauseMenu.Instance)
            if(PauseMenu.Instance.isPaused)
                return;

        Rotate();
        Move();

        if(isChild)
            CheckBorder();

        if(childrenAstroids != null) {
            foreach(var c in childrenAstroids) {
                if(c != null)
                    return;
            }
            //all children killed, award xp, destroy parent
            Instantiate(XPStar, transform.position, MyHelpers.RandomZRotation).InitializePickUp(childrenAstroids.Length);
            Destroy(gameObject);
        }
    }

    private void Move() {
        transform.position += moveSpeed * Time.deltaTime * direction;
    }

    private void Rotate() {
        transform.Rotate(rotateSpeed * Time.deltaTime * transform.forward);
    }

    public void Damage(float damage_) {
        if(isDead)
            return;

        currentHP -= damage_;
        StartCoroutine(DamageFlash());
        if(currentHP <= 0)
            Die();
    }

    IEnumerator DamageFlash() {
        spriteRenderer.color = Color.red;
        yield return StartCoroutine(MyHelpers.WaitForTime(0.05f));
        spriteRenderer.color = Color.white;
    }

    void Die() {
        isDead = true;

        AudioManager.Instance.PlaySound("Boom");
        AsteroidSpawner.Instance.GetAsteroids().Remove(this);
        var e = Instantiate(explosionPrefab, transform.position, MyHelpers.RandomZRotation);
        e.transform.localScale += Vector3.one * transform.localScale.magnitude / 2f;
        Destroy(e, 1.5f);

        if(isChild) {
            Destroy(gameObject);
            return;
        }

        //stop parent motion and collisions
        spriteRenderer.enabled = false;
        moveSpeed = rotateSpeed = 0;
        GetComponent<Collider2D>().enabled = false;

        childrenAstroids = new Asteroid[Random.Range(2, 5)];
        for(int i = 0; i < childrenAstroids.Length; i++) {
            childrenAstroids[i] = Instantiate(childPrefab, transform.position, MyHelpers.RandomZRotation);
            childrenAstroids[i].InitializeChild(this);
            childrenAstroids[i].transform.SetParent(transform);
            AsteroidSpawner.Instance.GetAsteroids().Add(childrenAstroids[i]);
        }
    }

    public void InitializeChild(Asteroid parent_) {
        isChild = true;
        startHP = currentHP = Mathf.Ceil(parent_.startHP / 5f);
        transform.localScale = Vector3.one * Random.Range(0.35f, 0.55f);
        transform.position += (Vector3) Random.insideUnitCircle * Random.Range(1f, 1.5f);
        moveSpeed = Random.Range(0.45f, 0.75f);
        rotateSpeed = Random.Range(3f, 8f);
    }

    Vector2 camExtents;
    public void CheckBorder() {
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

    private void OnTriggerEnter2D(Collider2D collision) {
        if(isDead)
            return;

        if(collision.TryGetComponent(out Player p)) {
            p.TakeDamage(LevelStats.Level);

            Vector3 dir = (p.transform.position - transform.position).normalized;
            p.GetComponent<Movement>().Knockback(dir, 20, 0.05f);
        }
    }
}
