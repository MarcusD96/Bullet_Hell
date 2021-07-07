
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [Header("Screen Bounds")]
    public float upperBound;
    public float lowerBound, leftBound, rightBound;

    public Enemy enemyPrefab;

    private void Start() {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies() {
        while(true) {
            Vector2 pos = new Vector2(Random.Range(leftBound, rightBound), Random.Range(lowerBound, upperBound));
            Enemy e = Instantiate(enemyPrefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(2.0f);
        }
    }
}
