using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAttack : ProjectileAttack {

    [Tooltip("total angle of spread")]
    public float spreadDegree;
    [Tooltip("odd number for symmetry")]
    public int numPellets;

    private float angleStep;

    private void Start() {
        angleStep = spreadDegree / numPellets;
    }

    public override void FireProjectiles(Transform[] fireSpawns_) {
        foreach(var f in fireSpawns_) {
            //generate numPellet rotations
            Vector3[] rots = new Vector3[numPellets];

            //make all rots equal to fireSpawn rot
            for(int i = 0; i < numPellets; i++) {
                rots[i] = f.rotation.eulerAngles;
            }

            //assign rotations
            int step = Mathf.FloorToInt((float) numPellets / 2f);
            for(int i = 0; i < numPellets; i++) {
                rots[i].z += angleStep * step;
                step--;
            }

            //shoot projectiles at desired rotation
            Quaternion saveRot = f.rotation;
            for(int i = 0; i < numPellets; i++) {
                f.rotation = Quaternion.Euler(rots[i]);
                var p = Instantiate(projectilePrefab, f.position, f.rotation);
                p.Initialize(damage, projSpeed + Random.Range(-0.5f, 0.5f), f.right);
            }

            //reset original rotation
            f.rotation = saveRot;
        }
    }
}