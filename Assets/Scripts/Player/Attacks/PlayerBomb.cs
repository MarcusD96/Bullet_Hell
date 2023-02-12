using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : PlayerProjectile {

    public LayerMask enemyLayer;
    public GameObject explosionEffect;

    protected Vector2 targetPos;
    protected Player player;
    protected bool isDead = false;

    private bool isCluster = false;

    private new void Start() {
        //if(isCluster == false)
        //GetActualTarget(FindObjectOfType<TargetCursor>().target.transform.position);

        player = FindObjectOfType<Player>();
        targetPos = Vector2.positiveInfinity;
    }

    public override void InitializeWithPenetrate(float damage_, float speed_, int penetration_, Vector2 direction_, Player owner_) {
        base.InitializeWithPenetrate(damage_, speed_, penetration_, direction_, owner_);
        //if(!isCluster)
        //GetActualTarget(FindObjectOfType<TargetCursor>().target.transform.position);
        //else
        targetPos = Vector2.positiveInfinity;
    }

    protected override void Update() {
        if(isDead == true)
            return;

        base.UpdatePathing();
        if(isCluster == false)
            if(Vector2.Distance(transform.position, targetPos) < 0.1f) {
                Explode(0.5f * player.penetration);
            }
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out Asteroid a)) {
            if(collision.TryGetComponent(out Enemy e)) {
                if(!e.isDead) {
                    Explode(0.5f * player.penetration);
                    return;
                }
            }
            if(a.isDead)
                return;
            Explode(0.5f * player.penetration);
        }
    }

    protected void Explode(float radius) {
        var randRot = Vector3.forward * Random.Range(0f, 360f);

        var e = Instantiate(explosionEffect, transform.position, Quaternion.Euler(randRot));
        e.transform.localScale = radius * Vector2.one;
        e.transform.GetChild(0).localScale = radius / 2f * Vector2.one;
        Destroy(e, 1f);

        var hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        foreach(var h in hits) {
            if(h.TryGetComponent(out Enemy ee))
                ee.GetDamaged(damage * 0.75f);
            else if(h.TryGetComponent(out Asteroid a))
                a.Damage(damage * 0.75f);
        }
        isDead = true;

        Destroy(gameObject, 0.5f);
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().Sleep();
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public void SetCluster() {
        isCluster = true;
    }

    void GetActualTarget(Vector3 targetPos_) {
        float mag = (targetPos_ - transform.position).magnitude;
        targetPos = transform.position + (transform.up * mag);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetPos);
    }
}
