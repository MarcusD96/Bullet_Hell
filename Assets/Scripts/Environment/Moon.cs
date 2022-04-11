using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour {

    [HideInInspector]
    public Transform sprite;

    private float rotateSpeed;

    private Transform parentPivot;

    private void Start() {
        rotateSpeed = Random.Range(2f, 8f);
        parentPivot = transform.parent;
        sprite = transform.GetChild(0);
        sprite.localScale = Vector2.one * Random.Range(0.25f, 1f);
    }

    private void Update() {
        if(PauseMenu.Instance)
            if(PauseMenu.Instance.isPaused)
                return;

        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }

}
