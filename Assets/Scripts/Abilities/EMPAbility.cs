using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPAbility : Ability {

    [SerializeField] GameObject empPrefab;

    protected override void OnActivate() {
        Instantiate(empPrefab, transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("ProjectileDump").transform);
    }
}
