using System.Collections;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {

    public bool canRandomSwitch = false;

    [Header("Background")]
    public float changeSpeed;
    public float moveSpeed;

    public Sprite[] sprites;
    public SpriteRenderer currentSR, nextSR;

    [Space(5)]
    [Header("Planets")]
    public Transform planetSpawn;
    public Planet[] planets;

    private int current;

    private void Start() {
        currentSR.sprite = sprites[Random.Range(0, sprites.Length)];
        SpawnPlanet();

        if(canRandomSwitch)
            InvokeRepeating(nameof(GetNextBackground), 0, 15);
    }

    private void Update() {
        if(PauseMenu.Instance)
            if(PauseMenu.Instance.isPaused)
                return;

        if(canRandomSwitch) {
            ScrollBackground();
            return;
        }

        MoveBackgroundWithGunner();
    }

    void MoveBackgroundWithGunner() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 offset = Time.deltaTime * moveSpeed * new Vector2(x + Time.deltaTime * moveSpeed, y);
        currentSR.sharedMaterial.mainTextureOffset = 
            Vector2.Lerp(currentSR.sharedMaterial.mainTextureOffset, currentSR.sharedMaterial.mainTextureOffset + offset, Time.deltaTime * moveSpeed);
    }

    void ScrollBackground() {
        Vector2 offset = Time.deltaTime * moveSpeed * Vector3.left;
        currentSR.sharedMaterial.mainTextureOffset =
            Vector2.Lerp(currentSR.sharedMaterial.mainTextureOffset, currentSR.sharedMaterial.mainTextureOffset + offset, Time.deltaTime * moveSpeed);
    }

    public void GetNextBackground() {
        int r = Random.Range(0, sprites.Length);

        if(r == current)
            r++;

        if(r >= sprites.Length)
            r = 0;

        current = r;
        nextSR.sprite = sprites[r];

        StartCoroutine(FadeSprites());
    }

    private void OnApplicationQuit() {
        currentSR.sharedMaterial.mainTextureOffset = Vector2.zero;
    }

    IEnumerator FadeSprites() {
        Color next = nextSR.color;

        float t = 0;
        float dt = Time.deltaTime * changeSpeed;

        while(t < 1f) {
            t += dt;

            next.a += dt;
            nextSR.color = next;

            yield return null;
        }

        currentSR.sprite = nextSR.sprite;
        nextSR.sprite = null;

        currentSR.color = Color.white;
        nextSR.color = Color.white - new Color(0, 0, 0, 1);
    }

    void SpawnPlanet() {
        Vector2 spawn = planetSpawn.position + (Vector3.up * Random.Range(-3f, 3f));

        var p = Instantiate(planets[Random.Range(0, planets.Length)], spawn, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        p.transform.localScale = Vector3.one * Random.Range(0.5f, 2f);
        p.transform.SetParent(planetSpawn, true);

        Invoke(nameof(SpawnPlanet), Random.Range(30, 240));
    }
}