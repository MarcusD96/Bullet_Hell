using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostAbility : Ability {

    [SerializeField] float useTime, speedScalar;

    protected override void OnActivate() {
        StartCoroutine(SpeedBoost());
    }

    IEnumerator SpeedBoost() {
        isUsingAbility = true;

        var player = GetComponent<Player>();

        player.baseMoveSpeed *= speedScalar;
        player.moveSpeed *= speedScalar;

        yield return MyHelpers.WaitForTime(useTime);

        player.baseMoveSpeed /= speedScalar;
        player.moveSpeed /= speedScalar;

        isUsingAbility = false;
    }
}
