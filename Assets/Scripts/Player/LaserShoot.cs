
using UnityEngine;

public class LaserShoot : MonoBehaviour {

    public Transform pivot, fireSpawn;
    public string shootStartSound, shootLoopSound, shootEndSound;
    public LineRenderer lr;
    public LayerMask enemyLayer;
    public ParticleSystem hitEffect, barrelEffect;
    public float shrinkSpeed, growSpeed;
    [Tooltip("1 / num seconds until next hit")]

    private float laserSize;
    private Player player;
    private Transform targetCursor;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Start() {
        targetCursor = FindObjectOfType<TargetCursor>().target.transform;
        laserSize = lr.startWidth;
    }

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        if(player.IsDead)
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
            AudioManager.Instance.PlaySound(shootEndSound);
            hitEffect.Stop();
            barrelEffect.Stop();
            if(shootingSoundIndex >= 0)
                AudioManager.Instance.StopSound(shootingSoundIndex);
        }

        hitEffect.gameObject.SetActive(false);
        barrelEffect.gameObject.SetActive(false);

        if(!lr.enabled)
            return;

        if(lr.startWidth <= 0.01f) {
            lr.startWidth = lr.endWidth = 0;
            lr.enabled = false;
            return;
        }

        ShrinkLaser();
    }

    void ShrinkLaser() {
        float scaleStart = lr.startWidth;
        float scaleEnd = lr.endWidth;
        lr.startWidth = Mathf.Lerp(scaleStart, 0, shrinkSpeed * Time.deltaTime);
        lr.endWidth = Mathf.Lerp(scaleEnd, 0, shrinkSpeed * Time.deltaTime);
    }

    int shootingSoundIndex = -1;
    void GrowLaser() {
        if(!lr.enabled) {
            AudioManager.Instance.PlaySound(shootStartSound);
            shootingSoundIndex = AudioManager.Instance.PlayLoopedSound(shootLoopSound);
            lr.enabled = true;
        }

        if(lr.startWidth >= laserSize) {
            lr.startWidth = laserSize;
            lr.endWidth = laserSize / 3;
            return;
        }

        float scaleStart = lr.startWidth;
        float scaleEnd = lr.endWidth;
        if(scaleStart < laserSize) {
            lr.startWidth = Mathf.Lerp(scaleStart, laserSize, growSpeed * Time.deltaTime);
            lr.endWidth = Mathf.Lerp(scaleEnd, laserSize / 3, growSpeed * Time.deltaTime);
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
        Vector3 dir = (GetForwardDestination(targetCursor.position) - pivot.position).normalized;
        //float desiredDestinationDist = Vector3.Distance(pivot.position, targetCursor.position);
        float maxDist = player.projectileSpeed * 0.75f;

        RaycastHit2D[] hits;
        Vector3 actualDestination;
        //if(desiredDestinationDist <= maxDist) {
        //    actualDestination = GetForwardDestination(targetCursor.position);
        //    hits = Physics2D.RaycastAll(pivot.position, dir, desiredDestinationDist, enemyLayer);
        //}
        //else {
            actualDestination = pivot.position + (dir * maxDist);
            hits = Physics2D.RaycastAll(pivot.position, dir, maxDist, enemyLayer);
        //}

        //less that max penetration hits, draw at max distance
        if(hits.Length < player.penetration) {
            DrawLine(fireSpawn.position, actualDestination);
            DamageEnemies(hits);
            return;
        }

        //hit max penetration, shrink array to penetration size, draw until last enemy hit point
        RaycastHit2D[] shrinkHits = new RaycastHit2D[player.penetration];
        for(int i = 0; i < player.penetration; i++) {
            shrinkHits[i] = hits[i];
        }
        DamageEnemies(shrinkHits);

        DrawLine(fireSpawn.position, shrinkHits[shrinkHits.Length - 1].point);
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
            if(e.collider.TryGetComponent(out Enemy ee))
                ee.GetDamaged(player.damage);
            else if(e.collider.TryGetComponent(out Asteroid a))
                a.Damage(player.damage);
        }
    }

    Vector3 GetForwardDestination(Vector3 t) {
        float mag = (t - fireSpawn.position).magnitude;
        return fireSpawn.position + (pivot.right * mag);
    }
}
