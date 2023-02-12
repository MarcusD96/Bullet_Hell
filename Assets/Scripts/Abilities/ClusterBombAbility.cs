using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBombAbility : Ability {

    [SerializeField] Weapon clusterBombPrefab;

    Weapon baseWeapon;
    PlayerShoot ps;

    private void Start() {
        ps = GetComponent<PlayerShoot>();
        baseWeapon = ps.weapon;
    }


    protected override void OnActivate() {
        ps.weapon = clusterBombPrefab;
        ps.FireOverride();
        ps.weapon = baseWeapon;
    }

}
