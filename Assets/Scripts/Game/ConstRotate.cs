using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstRotate : MonoBehaviour {

    [SerializeField] float rotateSpeed;

    private void Start() {
        if(Random.Range(0, 2) == 0)
            rotateSpeed = -rotateSpeed;
    }

    private void Update() {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);
    }

}
