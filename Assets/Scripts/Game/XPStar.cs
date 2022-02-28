
using UnityEngine;

public class XPStar : PickUpStar {

    public override bool PickUp() {
        if(!base.PickUp())
            return true;

        PlayerStatsManager.Instance.AddXP(points);
        Destroy(gameObject);

        return false;
    }

}
