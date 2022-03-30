
using System.Collections;
using UnityEngine;

public class QuickShoot : PlayerShoot {

    [SerializeField] int burstNum;

    protected override void Fire() {
        StartCoroutine(BurstFire());
    }

    IEnumerator BurstFire() {
        for(int i = 0; i < burstNum; i++) {
            AudioManager.Instance.PlaySound(shootSound);
            foreach(var f in player.fireSpawns) {
                Weapon b = Instantiate(weapon, f.position, Quaternion.identity);
                b.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileDump").transform);
                Vector2 dir = (f.position - transform.position).normalized;
                b.InitializeWithPenetrate(player.damage, player.projectileSpeed, player.penetration, dir);
            }
            float waitTime = (1 / (player.fireRate * 2f)) / burstNum;
            yield return MyHelpers.WaitForTime(waitTime);
        }
    }

}
