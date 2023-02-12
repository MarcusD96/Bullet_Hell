using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBomb : PlayerBomb {

    [SerializeField] PlayerBomb clusterBombs;
    [SerializeField] int clusters;

    protected override void Update() {
        if(isDead == true)
            return;

        base.UpdatePathing();
        if(Vector2.Distance(transform.position, targetPos) < 0.1f) {
            base.Explode(0.25f * player.penetration);
            StartCoroutine(Cluster());
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out Enemy _) || collision.TryGetComponent(out Asteroid _)) {
            base.Explode(0.25f * player.penetration);
            StartCoroutine(Cluster());
        }
    }

    IEnumerator Cluster() {
        yield return MyHelpers.WaitForTime(0.1f);

        for(int i = 0; i < clusters; i++) {

            float radians = 2 * Mathf.PI / clusters * i;
            var spawnDir = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
            var spawnPos = transform.position;

            PlayerBomb b = Instantiate(clusterBombs, spawnPos, Quaternion.identity);
            b.gameObject.transform.localScale *= 0.75f;
            b.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileDump").transform);
            b.InitializeWithPenetrate(player.damage * 0.25f, player.projectileSpeed * 4f, Mathf.CeilToInt(player.penetration * 0.25f), spawnDir, owner);
            b.SetCluster();
            yield return MyHelpers.WaitForTime(Time.deltaTime);
        }
    }
}
