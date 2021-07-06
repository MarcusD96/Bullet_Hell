
using UnityEngine;

public class Shoot : MonoBehaviour {

    public Bullet bullet;
    public Transform pivot, fireSpawn;

    // Update is called once per frame
    void Update() {
        ShootInput();
    }

    void ShootInput() {
        if(Input.GetMouseButton(0))
            Fire();
    }

    public float firerate;
    float nextfire = 0;
    void Fire() {
        if(nextfire <= 0) {
            Bullet b = Instantiate(bullet, fireSpawn.position, Quaternion.identity);
            b.Initialize(pivot.rotation * Vector2.right);
            nextfire = 1 / firerate;
        }
        nextfire -= Time.deltaTime;
    }
}
