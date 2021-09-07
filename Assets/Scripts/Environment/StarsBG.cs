using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsBG : MonoBehaviour {

    public Star[] stars;
    public float spawnRate;

    private float nextSpawn = 0;
    List<Star> starList = new List<Star>();

    private void Start() {
        StartCoroutine(CreateStarPool(10));
    }

    private void LateUpdate() {
        if(nextSpawn <= 0) {
            int starId = Random.Range(0, starList.Count);

            CheckStar(starId);

            nextSpawn = 1 / spawnRate;
        }
        nextSpawn -= Time.deltaTime;
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
            if(fallback > 20)
                break;
        }
        ActivateStar(id);
    }

    void ActivateStar(int id) {
        starList[id].gameObject.SetActive(true);
        starList[id].SetStar();
    }

    IEnumerator CreateStarPool(int num) {
        foreach(var s in stars) {
            for(int i = 0; i < num; i++) {
                Star g = Instantiate(s, transform);
                g.gameObject.SetActive(false);
                starList.Add(g);
                yield return null;
            }
        }
    }
}
