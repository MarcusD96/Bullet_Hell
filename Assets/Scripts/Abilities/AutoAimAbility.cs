using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimAbility : Ability {

    [SerializeField] float useTime;
    [SerializeField] GameObject radiusSprite;
    [SerializeField] Weapon autoBullet;

    Movement move;
    PlayerShoot ps;
    PlayerAutoAim paa;

    private void Start() {
        ps = GetComponent<PlayerShoot>();
        move = GetComponent<Movement>();
        paa = GetComponent<PlayerAutoAim>();

        radiusSprite.transform.localScale = Vector3.one * (paa.detectionRange * 2 /  transform.localScale.x);
        radiusSprite.SetActive(false);
    }

    protected override void OnActivate() {
        StartCoroutine(AutoAim());
    }

    IEnumerator AutoAim() {
        isUsingAbility = true;

        var saveWep = ps.weapon;
        ps.weapon = autoBullet;

        radiusSprite.SetActive(true);
        ps.ToggleAuto();
        move.ToggleAuto();
        paa.enabled = true;

        yield return MyHelpers.WaitForTime(useTime);

        ps.weapon = saveWep;
        radiusSprite.SetActive(false);
        ps.ToggleAuto();
        move.ToggleAuto();
        paa.enabled = false;

        isUsingAbility = false;
    }
}
