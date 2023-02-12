
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public string shootSound;
    public Weapon weapon;
    public MuzzleFlash weaponFlash;
    public Transform pivot;

    [HideInInspector] public Weapon firedWeapon;

    protected Player player;

    int shotsFired = 0;
    bool auto = false;

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
        if(auto)
            return;

        if(Input.GetButton("Fire1") || Input.GetAxis("Fire1") > 0) {
            CheckFire();
        }
    }

    public void CheckFire() {
        if(player.nextFire <= 0) {
            Fire();
            player.nextFire = 1 / player.fireRate;
        }
    }

    protected virtual void Fire() {
        AudioManager.Instance.PlaySound(shootSound);

        shotsFired++;
        foreach(var f in player.fireSpawns) {
            firedWeapon = Instantiate(weapon, f.position, Quaternion.identity, GameObject.FindGameObjectWithTag("ProjectileDump").transform);
            Vector2 dir = (f.position - transform.position).normalized;
            firedWeapon.InitializeWithPenetrate(player.damage, player.projectileSpeed, player.penetration, dir, player);

            if(weaponFlash) {
                var g  = Instantiate(weaponFlash, f.position, f.rotation);
                g.transform.SetParent(f, true);
                g.SetLayer(true);
            }
        }
    }

    public void FireOverride() {
        Fire();
    }

    public int GetShots() {
        return shotsFired;
    }

    public void ToggleAuto() {
        auto = !auto;
    }
}
