
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public string shootSound;
    public Weapon weapon;
    public Transform pivot;

    protected Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        if(player.IsDead)
            return;

        ShootInput();
        player.nextFire -= Time.deltaTime;
    }

    private void ShootInput() {
        if(Input.GetButton("Fire1") || Input.GetAxis("Fire1") > 0) {
            CheckFire();
        }
    }

    private void CheckFire() {
        if(player.nextFire <= 0) {
            Fire();
            player.nextFire = 1 / player.fireRate;
        }
    }

    protected virtual void Fire() {
        AudioManager.Instance.PlaySound(shootSound);
        foreach(var f in player.fireSpawns) {
            Weapon b = Instantiate(weapon, f.position, Quaternion.identity);
            b.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileDump").transform);
            Vector2 dir = (f.position - transform.position).normalized;
            b.InitializeWithPenetrate(player.damage, player.projectileSpeed, player.penetration, dir);
        }
    }
}
