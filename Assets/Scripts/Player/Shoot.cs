
using UnityEngine;

public class Shoot : MonoBehaviour {

    public Weapon bullet;
    public Transform pivot;

    private Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        ShootInput();
        player.nextFire -= Time.deltaTime;
    }

    private void ShootInput() {
        if(Input.GetButton("Fire1") || Input.GetAxis("Fire1") > 0) {
            Fire();
        }
    }

    private void Fire() {
        if(player.nextFire <= 0) {

            foreach(var f in player.fireSpawns) {
                Weapon b = Instantiate(bullet, f.position, Quaternion.identity);
                Vector2 dir = (f.position - transform.position).normalized;
                b.Initialize_Penetrate(player.damage, player.projectileSpeed, player.penetration, dir, pivot.rotation);
            }

            player.nextFire = 1 / player.fireRate;
        }
    }
}
