using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstPulse : MonoBehaviour {

    [SerializeField] float pulseSpeed;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;

    private void Update() {
        var s = transform.localScale;

        if(s.x >= maxSize) {
            s.x = s.y = maxSize;
            pulseSpeed = -pulseSpeed;
        }
        else if(s.x <= minSize) {
            s.x = s.y = minSize;
            pulseSpeed = -pulseSpeed;
        }

        transform.localScale = s;
        Scale();
    }

    void Scale() {
        transform.localScale += pulseSpeed * new Vector3(1, 1, 0) * Time.deltaTime;
    }
}
