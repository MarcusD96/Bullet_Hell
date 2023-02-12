using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPShadowClone : MonoBehaviour {

    private GameObject owner;

    public void Initialize(GameObject owner_) {
        owner = owner_;
    }

    private void Update() {
        if(!owner)
            Destroy(gameObject);
    }

    public void Destroy() { 
        Destroy(gameObject);
    }

}
