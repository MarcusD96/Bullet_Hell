using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Weapon {

    public Transform sprite;
    public float burnTime;
    public Material[] fireMats;
    public float lifeTime, destroySpeed;

    bool hit = false;
    Enemy e;

    private void Start() {
        int r = Random.Range(0, fireMats.Length);
        sprite.GetComponent<SpriteRenderer>().sharedMaterial = fireMats[r];
        transform.localScale *= Random.Range(0.5f, 1.5f);
        StartCoroutine(EndOfLife());
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent(out e)) {
            if(!hit) {                
                if(penetration < 2) {
                    hit = true;
                    e.Burn(damage, burnTime);
                    Destroy(gameObject);
                }
                else {
                    e.Burn(damage, burnTime);
                    penetration--;
                }
            }
        }
    }

    IEnumerator EndOfLife() {
        yield return new WaitForSeconds(lifeTime);
        while(transform.localScale.x > 0) {
            transform.localScale -= Vector3.one * Time.deltaTime * destroySpeed;
            yield return null;
        }
        Destroy(gameObject);
    }

}
