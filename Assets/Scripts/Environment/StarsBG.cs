using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsBG : MonoBehaviour {

    public Star[] stars;
    public float spawnRate;
    public int poolSize;

    private float nextSpawn = 0;
    List<Star> starList = new List<Star>();

    private void Start() {
        PopulateScreen();
        StartCoroutine(CreateStarPool());
    }

    private void LateUpdate() {
        if(PauseMenu.Instance) {
            if(PauseMenu.Instance.isPaused)
                return;
        }

        if(nextSpawn <= 0) {
            int starId = Random.Range(0, starList.Count);

            CheckStar(starId);

            nextSpawn = 1 / spawnRate;
        }
        nextSpawn -= Time.deltaTime;
    }

    void PopulateScreen() {
        for(int i = 0; i < 15; i++) {
            int s = Random.Range(0, stars.Length - 1);
            var star_ = CreateStar(s, true);
            RandomizeStar(star_.gameObject);

            Vector2 randPos = new Vector2(Random.Range(-9f, 9f), Random.Range(-4.5f, 4.5f));
            star_.transform.position = randPos;

            starList.Add(star_);
        }
    }

    void CheckStar(int id) {
        int fallback = 0;
        while(true) {
            //if its disabled, then its ready to spawm, leave loop
            if(!starList[id].gameObject.activeSelf)
                break;

            //it is enabled and currently being used, move to the next in list
            id++;
            if(id >= starList.Count)
                id = 0;

            fallback++;

            //create new star
            if(fallback > poolSize) {
                int i = Random.Range(0, stars.Length - 1);
                RandomizeStar(CreateStar(i, true).gameObject);
                break;
            }
        }
        ActivateStar(id);
    }

    void ActivateStar(int id) {
        var s = starList[id];

        s.gameObject.SetActive(true);
        s.transform.localScale = Vector3.one;

        RandomizeStar(s.gameObject);
    }

    void RandomizeStar(GameObject star_) {

        Vector2 pos = star_.transform.position;
        pos.x = 10;
        pos.y = Random.Range(-4.5f, 4.5f);
        star_.transform.position = pos;

        Vector3 euler = star_.transform.rotation.eulerAngles;
        euler.z = Random.Range(0, 360);
        star_.transform.rotation = Quaternion.Euler(euler);

        star_.transform.localScale *= Random.Range(0.25f, 4f);
    }

    Star CreateStar(int starID_, bool active_) {
        Star g = Instantiate(stars[starID_], transform);
        g.gameObject.SetActive(active_);
        starList.Add(g);
        return g;
    }

    IEnumerator CreateStarPool() {
        for(int i = 0; i < stars.Length; i++) {
            for(int ii = 0; ii < poolSize; ii++) {
                CreateStar(i, false);
                yield return null;
            }
        }
    }
}
