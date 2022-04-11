using System.Collections;
using System.Collections.Generic;
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
    private bool isRapidScrolling = false;
    private List<Planet> spawnedPlanets = new List<Planet>();

    private void Start() {
        currentSR.sprite = sprites[Random.Range(0, sprites.Length)];

        Invoke(nameof(SpawnPlanet), 5);

        if(canRandomSwitch)
            Invoke(nameof(GetNextBackground), Random.Range(30f, 60f));
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
        if(isRapidScrolling)
            return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 offset = Time.deltaTime * moveSpeed * new Vector2(x + Time.deltaTime * moveSpeed, y);
        currentSR.sharedMaterial.mainTextureOffset =
            Vector2.Lerp(currentSR.sharedMaterial.mainTextureOffset, currentSR.sharedMaterial.mainTextureOffset + offset, Time.deltaTime * moveSpeed);
    }

    void ScrollBackground() {
        if(isRapidScrolling)
            return;

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
        StartCoroutine(RapidScroll());

        if(canRandomSwitch)
            Invoke(nameof(GetNextBackground), Random.Range(30f, 60f));        
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
        //weighted random num spawned
        int num;
        float r = (float) new System.Random().NextDouble();
        if(r <= 0.6f)
            num = 1;
        else if(r > 0.6f && r < 0.9f)
            num = 2;
        else
            num = 3;

        for(int i = 0; i < num; i++) {
            Vector2 spawn = MyHelpers.GetCameraBorderRandomPosition(0);

            var p = Instantiate(planets[Random.Range(0, planets.Length)], spawn, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            spawnedPlanets.Add(p);
            p.transform.localScale = MyHelpers.RandomXYScale(0.5f, 2f);
            p.transform.SetParent(planetSpawn);
            StartCoroutine(InvokePlanetEnd(75, p));
        }

        Invoke(nameof(SpawnPlanet), Random.Range(30, 150));
    }

    IEnumerator InvokePlanetEnd(float time_, Planet p) {
        yield return StartCoroutine(MyHelpers.WaitForTime(time_));
        RemovePlanet(p);
    }

    void RemovePlanet(Planet p) {
        if(!p)
            return;

        spawnedPlanets.Remove(p);
        Destroy(p.gameObject);
    }

    IEnumerator RapidScroll() {
        isRapidScrolling = true;

        float t = 0;
        float dt = Time.deltaTime * changeSpeed;
        float startMoveSpeed = moveSpeed;
        while(t < 1f) {
            t += dt;
            //speed up
            if(t < 0.2f)
                moveSpeed = Mathf.Lerp(moveSpeed, startMoveSpeed * 20f, Time.deltaTime);
            //slow down
            else if(t > 0.8f)
                moveSpeed = Mathf.Lerp(moveSpeed, startMoveSpeed, Time.deltaTime / changeSpeed);

            //move
            Vector2 offset = Time.deltaTime * moveSpeed * Vector3.left;
            currentSR.sharedMaterial.mainTextureOffset =
                Vector2.Lerp(currentSR.sharedMaterial.mainTextureOffset, currentSR.sharedMaterial.mainTextureOffset + offset, Time.deltaTime * moveSpeed);

            //move planets
            foreach(var p in spawnedPlanets) {
                p.SetDirection(Vector3.left);
                p.moveSpeed = Mathf.Lerp(p.moveSpeed, p.moveSpeed * 3f, Time.deltaTime);
            }

            yield return null;
        }
        moveSpeed = startMoveSpeed;

        for(int i = spawnedPlanets.Count - 1; i >= 0; i--) {
            Destroy(spawnedPlanets[i].gameObject);
        }
        spawnedPlanets.Clear();
        SpawnPlanet();

        isRapidScrolling = false;
    }
}