using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

    [SerializeField] Sprite[] sprites;
    [SerializeField] float flashTime;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        StartCoroutine(FlashSprite());
    }

    IEnumerator FlashSprite() {
        int r = Random.Range(0, sprites.Length);

        spriteRenderer.sprite = sprites[r];

        yield return MyHelpers.WaitForTime(flashTime);

        Destroy(gameObject);
    }

    public void SetLayer(bool isPlayer) {
        if(isPlayer)
            spriteRenderer.sortingLayerName = "Player";
        else
            spriteRenderer.sortingLayerName = "Enemy";
    }
}
