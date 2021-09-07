using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {

    public float speed;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        Vector2 offset = new Vector2(Time.time * speed, 0);
        spriteRenderer.sharedMaterial.mainTextureOffset = offset;
    }
}
