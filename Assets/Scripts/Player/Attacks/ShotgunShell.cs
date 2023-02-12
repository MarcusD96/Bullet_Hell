using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShell : Weapon {

    public int numProjectiles;
    public Weapon pellets;

    private new void Start() {
        base.Start();
        SpawnPellets();
        Destroy(gameObject);
    }

    void SpawnPellets() {
        for(int i = 0; i < numProjectiles; i++) {
            Weapon b = Instantiate(pellets, transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("ProjectileDump").transform);
            b.InitializeWithPenetrate(damage, speed, penetration, direction, owner);
        }
    }
}
