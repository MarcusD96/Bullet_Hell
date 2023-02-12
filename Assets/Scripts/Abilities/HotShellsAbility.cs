using System.Collections;
using UnityEngine;

public class HotShellsAbility : Ability {

    [SerializeField] int loadedShots;
    [SerializeField] ShotgunShell hotShell;

    PlayerShoot ps;

    private void Start() {
        ps = GetComponent<PlayerShoot>();
    }

    protected override void OnActivate() {
        StartCoroutine(LoadHotShells());
    }

    IEnumerator LoadHotShells() {
        isUsingAbility = true;

        int startShots = ps.GetShots();

        var saveWep = ps.weapon;
        ps.weapon = hotShell;

        while(ps.GetShots() < startShots + loadedShots) {
            yield return null;
        }

        ps.weapon = saveWep;

        isUsingAbility = false;
    }
}
