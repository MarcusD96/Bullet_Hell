using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStar : PickUpStar {

    public override bool PickUp() {
        if(!base.PickUp())
            return false;

        //award upgrade token
        UpgradeManager.Instance.AwardUpgradeToken();
        Destroy(gameObject);

        return true;
    }

}
