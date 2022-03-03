
using UnityEngine;

public class LaserShoot : MonoBehaviour {

    public Transform pivot;
    public LineRenderer lr;
    public LayerMask enemyLayer;
    public ParticleSystem hitEffect, barrelEffect;
    public float shrinkSpeed, growSpeed;
    [Tooltip("1 / num seconds until next hit")]

    private Player player;
    private Transform targetCursor;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Start() {
        targetCursor = FindObjectOfType<TargetCursor>().target.transform;
    }

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        ShootInput();
        player.nextFire -= Time.deltaTime;
    }

    void ShootInput() {
        if(Input.GetButton("Fire1") || Input.GetAxis("Fire1") > 0) {
            FireLaser();
        }
        else
            LaserOff();
    }

    private void LaserOff() {
        if(hitEffect.isPlaying) {
            hitEffect.Stop();
            barrelEffect.Stop();
        }
        hitEffect.gameObject.SetActive(false);
        barrelEffect.gameObject.SetActive(false);

        if(!lr.enabled)
            return;

        if(lr.startWidth <= 0f) {
            lr.startWidth = lr.endWidth = 0;
            lr.enabled = false;
            return;
        }

        ShrinkLaser();
    }

    void ShrinkLaser() {
        float scale = lr.startWidth;
        lr.startWidth = lr.endWidth = Mathf.Lerp(scale, 0, shrinkSpeed * Time.deltaTime);
    }

    void GrowLaser() {
        if(!lr.enabled)
            lr.enabled = true;

        if(lr.startWidth >= 0.2f) {
            lr.startWidth = lr.endWidth = 0.2f;
            return;
        }

        float scale = lr.startWidth;
        if(scale < 0.2f) {
            lr.startWidth = lr.endWidth = Mathf.Lerp(scale, 0.2f, growSpeed * Time.deltaTime);
        }
    }

    void FireLaser() {
        hitEffect.gameObject.SetActive(true);
        barrelEffect.gameObject.SetActive(true);
        GrowLaser();
        CheckLaser();
    }

    private void CheckLaser() {
        //only use main fireSpawn
        Vector3 dir = (targetCursor.position - pivot.position).normalized;
        float desiredDestinationDist = Vector3.Distance(pivot.position, targetCursor.position);
        float maxDist = player.projectileSpeed * 0.75f;

        RaycastHit2D[] hits;
        Vector3 actualDestination;
        if(desiredDestinationDist <= maxDist) {
            actualDestination = targetCursor.position;
            hits = Physics2D.RaycastAll(pivot.position, dir, desiredDestinationDist, enemyLayer);
        }
        else {
            actualDestination = pivot.position + (dir * maxDist);
            hits = Physics2D.RaycastAll(pivot.position, dir, maxDist, enemyLayer);
        }

        //less that max penetration hits, draw at max distance
        if(hits.Length < player.penetration) {
            DrawLine(pivot.position, actualDestination);
            DamageEnemies(hits);
            return;
        }

        //hit max penetration, shrink array to penetration size, draw until last enemy hit point
        RaycastHit2D[] shrinkHits = new RaycastHit2D[player.penetration];
        for(int i = 0; i < player.penetration; i++) {
            shrinkHits[i] = hits[i];
        }
        DamageEnemies(shrinkHits);

        DrawLine(pivot.position, shrinkHits[shrinkHits.Length - 1].point);
    }

    void DrawLine(Vector2 startPos, Vector2 endPos) {
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        hitEffect.transform.position = endPos;
        if(!hitEffect.isPlaying) {
            hitEffect.Play();
            barrelEffect.Play();
        }
    }

    void DamageEnemies(RaycastHit2D[] enemies) {
        //check to see if laser is ready to hit targets
        if(player.nextFire > 0)
            return;

        player.nextFire = 1 / player.fireRate;

        //damage all enemies in raycast array
        foreach(var e in enemies) {
            e.collider.GetComponent<Enemy>().GetDamaged(player.damage);
        }
    }
}
