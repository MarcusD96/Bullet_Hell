using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedBolt : Weapon {

    public float maxAngularSpeed;

    private GameObject target;
    private bool hit = false;
    private List<GameObject> hitEnemies = new List<GameObject>();

    new void Start() {
        base.Start();
        maxAngularSpeed = speed * 30;
    }

    private void FixedUpdate() {
        if(PauseMenu.Instance.isPaused)
            return;

        FindEnemy();

        if(target)
            FindLimitedRotation();
    }

    void FindLimitedRotation() {
        float dt = Time.deltaTime;
        Vector3 dir = (target.transform.position - transform.position).normalized;
        Vector3 pos = transform.position;

        //cos-1(A . B / |A||B|)
        float angleDesired = Mathf.Acos(Vector3.Dot(dir, direction) / direction.magnitude) * Mathf.Rad2Deg; //dir is normalized already, so 1
        float angleActual = maxAngularSpeed * dt;

        if(angleDesired <= angleActual) {
            direction = dir.normalized;
        }
        else {
            Vector2 n = new Vector2(direction.y, -direction.x);
            float a = Vector2.Dot(n, dir);
            if(a > 0) {
                angleActual *= -1;
            }
            angleActual *= Mathf.Deg2Rad;
            dir.x = (direction.x * Mathf.Cos(angleActual)) - (direction.y * Mathf.Sin(angleActual));
            dir.y = (direction.x * Mathf.Sin(angleActual)) + (direction.y * Mathf.Cos(angleActual));
            direction = dir.normalized;
        }
        UpdateRotation();
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out Enemy e)) {
            if(!hit) {
                if(penetration < 2) {
                    hit = true;
                    e.GetDamaged(damage);
                    Destroy(gameObject);
                }
                else {
                    e.GetDamaged(damage);
                    penetration--;
                }
                hitEnemies.Add(e.gameObject);
                target = null;
            }
        }
        if(collision.TryGetComponent(out Asteroid a)) {
            if(!hit) {
                if(penetration < 2) {
                    hit = true;
                    a.Damage(damage);
                    Destroy(gameObject);
                }
                else {
                    a.Damage(damage);
                    penetration--;
                }
                hitEnemies.Add(a.gameObject);
                target = null;
            }
        }
    }

    void FindEnemy() {
        //find closest enemy
        float closestDistance = 100;
        GameObject closestHitable = null;

        List<GameObject> hitables = new List<GameObject>();
        foreach(var e in EnemySpawner.Instance.GetSpawnedEnemies()) {
            hitables.Add(e.gameObject);
        }
        foreach(var a in AsteroidSpawner.Instance.GetAsteroids()) {
            hitables.Add(a.gameObject);
        }

        foreach(var h in hitables) {
            if(h == null)
                continue;
            if(hitEnemies.Contains(h))
                continue;
            if(h.TryGetComponent(out BossEnemy e))
                if(e.IsInvincible())
                    continue;
                

            var d = Vector2.Distance(h.transform.position, transform.position);
            if(d < closestDistance) {
                closestDistance = d;
                closestHitable = h;
            }
        }

        if(closestHitable)
            target = closestHitable;
    }
}
