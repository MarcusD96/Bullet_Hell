using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : PlayerProjectile {

    public LayerMask enemyLayer;
    public GameObject explosionEffect;

    Player player;

    Vector2 targetPos;

    private new void Start() {
        targetPos = FindObjectOfType<TargetCursor>().target.transform.position;
        player = FindObjectOfType<Player>();
    }

    protected override void Update() {
        base.Update();
        if(Vector2.Distance(transform.position, targetPos) < 0.1f)
            Explode();
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out Enemy _) || collision.TryGetComponent(out Asteroid _))
            Explode();
    }

    void Explode() {
        var randRot = Vector3.forward * Random.Range(0f, 360f);
        float expRad = 0.5f * player.penetration;

        var e = Instantiate(explosionEffect, transform.position, Quaternion.Euler(randRot));
        e.transform.localScale = expRad * Vector2.one;
        e.transform.GetChild(0).localScale = expRad / 2f * Vector2.one;
        Destroy(e, 1f);

        var hits = Physics2D.OverlapCircleAll(transform.position, expRad, enemyLayer);
        foreach(var h in hits) {
            if(h.TryGetComponent(out Enemy ee))
                ee.GetDamaged(damage);
            else if(h.TryGetComponent(out Asteroid a))
                a.Damage(damage);
        }
        Destroy(gameObject);
    }

}
