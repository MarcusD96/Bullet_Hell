using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedBolt : Weapon {

    public float maxAngularSpeed;

    private Enemy target, hitEnemy;
    private bool hit = false;

    private void FixedUpdate() {
        FindEnemy();
        if(target) {
            FindLimitedRotation();
        }
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
        if(collision.TryGetComponent(out hitEnemy)) {
            if(!hit) {
                if(penetration < 2) {
                    hit = true;
                    hitEnemy.GetDamaged(damage);
                    Destroy(gameObject);
                }
                else {
                    hitEnemy.GetDamaged(damage);
                    penetration--;
                }
            }
        }
    }

    void FindEnemy() {
        if(target == null) {
            //find closest enemy
            float closestDistance = float.MaxValue;
            Enemy closestEnemy = null;

            foreach(var e in EnemySpawner.Instance.GetSpawnedEnemies()) {
                if(e == null)
                    continue;
                var d = Vector2.Distance(e.transform.position, transform.position);
                if(d < closestDistance) {
                    d = closestDistance;
                    closestEnemy = e;
                }
            }

            if(closestEnemy)
                target = closestEnemy;
        }
    }
}
