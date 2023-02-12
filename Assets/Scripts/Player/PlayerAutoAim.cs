using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoAim : MonoBehaviour {

    public float detectionRange;

    [SerializeField] float minFocusTime;

    float currentFocusTime = 0;
    Transform pivot;
    PlayerShoot ps;
    Enemy targetEnemy;
    Asteroid targetAsteroid;

    bool firstStart = true;

    private void Awake() {
        pivot = GetComponent<Movement>().pivot;
        ps = GetComponent<PlayerShoot>();
    }

    private void OnEnable() {
        if(firstStart) {
            firstStart = false;
            enabled = false;
            return;
        }

        FindClosestEnemy();
    }

    private void OnDisable() {
        currentFocusTime = 0;
        targetAsteroid = null;
        targetEnemy = null;
    }

    private void Update() {
        if(targetEnemy == null) {
            FindClosestEnemy();

            if(targetEnemy == null) {
                FindClosestAsteroid();

                if(targetAsteroid == null)
                    return;
            }
        }

        if(Time.time >= currentFocusTime + minFocusTime) {
            FindClosestEnemy();
            return;
        }

        if(IsTooFar() == true) {
            FindClosestEnemy();
            return;
        }

        RotateToTarget();
        FireAtTarget();
    }

    void FindClosestEnemy() {
        targetEnemy = null;

        var enemies = EnemySpawner.Instance.GetSpawnedEnemies();

        if(enemies.Count < 1)
            return;

        //sort list by distance to transform
        enemies.Sort(delegate (Enemy a, Enemy b) {
            return Vector2.Distance(transform.position, a.transform.position)
                   .CompareTo(
                   Vector2.Distance(transform.position, b.transform.position));
        });


        if(enemies.Count > 0) {
            var e = enemies[0];

            if(Vector2.Distance(transform.position, e.transform.position) <= detectionRange + e.GetRadius()) {
                targetEnemy = e;
                currentFocusTime = Time.time;
            }
        }
    }

    void FindClosestAsteroid() {
        targetAsteroid = null;

        var asteroids = AsteroidSpawner.Instance.GetAsteroids();

        asteroids.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position)
                 .CompareTo(
                 Vector2.Distance(transform.position, b.transform.position))
        );

        if(asteroids.Count > 0) {
            var a = asteroids[0];

            if(Vector2.Distance(transform.position, a.transform.position) <= detectionRange + a.GetRadius()) {
                targetAsteroid = a;
                currentFocusTime = Time.time;
            }
        }
    }

    void RotateToTarget() {
        Transform target;

        if(targetEnemy)
            target = targetEnemy.transform;
        else
            target = targetAsteroid.transform;

        Vector3 targetRot = (target.position - pivot.position).normalized;
        float dist = Vector3.Distance(pivot.right, targetRot);
        dist = Mathf.Clamp(dist, 0, 0.5f);

        pivot.right = Vector3.Lerp(pivot.right, targetRot, (4f / dist) * Time.deltaTime);
    }

    void FireAtTarget() {
        ps.CheckFire();
    }

    bool IsTooFar() {
        if(targetEnemy)
            if(Vector2.Distance(transform.position, targetEnemy.transform.position) > detectionRange + targetEnemy.GetRadius())
                return true;
        else if(targetAsteroid)
            if(Vector2.Distance(transform.position, targetAsteroid.transform.position) > detectionRange + targetAsteroid.GetRadius())
                return true;

        return false;
    }


    private void OnDrawGizmosSelected() {
        if(enabled == false)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        float r;

        if(targetEnemy)
            r = targetEnemy.GetRadius();
        else if(targetAsteroid)
            r = targetAsteroid.GetRadius();
        else
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange + r);
    }
}
