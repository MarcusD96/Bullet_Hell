using System.Collections;
using UnityEngine;

public class ShieldAbility : Ability {

    [SerializeField] GameObject shieldBubble;
    [SerializeField] float shieldTime;

    private void Start() {
        shieldBubble.SetActive(false);
    }

    protected override void OnActivate() {
        StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield() {
        isUsingAbility = true;

        shieldBubble.SetActive(true);
        GetComponentInParent<Player>().SetInvulnerable();

        yield return MyHelpers.WaitForTime(shieldTime);

        shieldBubble.SetActive(false);
        GetComponentInParent<Player>().SetInvulnerable();

        isUsingAbility = false;
    }
}
