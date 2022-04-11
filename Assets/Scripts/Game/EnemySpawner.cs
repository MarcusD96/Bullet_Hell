
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public static EnemySpawner Instance;

    [Header("Screen Bounds")]
    public float verticalBound;
    public float horizontalBound;

    [SerializeField]
    public List<Wave> waves;

    private List<Enemy> spawnedEnemies;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("More than 1 spawner...deleting this one");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void StartSpawning() {
        spawnedEnemies = new List<Enemy>();
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies() {
        yield return StartCoroutine(MyHelpers.WaitForTime(2f));

        for(int i = 0; i < waves.Count; i++) {
            foreach(var c in waves[i].chunks) {
                StartCoroutine(SpawnWaveChunk(c));
            }

            //check for game pause
            yield return StartCoroutine(MyHelpers.WaitForTime(waves[i].GetChunkTime()));

            while(spawnedEnemies.Count > 0) {
                yield return null;
            }
            FindObjectOfType<BackgroundScroll>().GetNextBackground();
            LevelStats.Level++;
        }
        SceneFader.Instance.FadeToScene(0);
    }

    //bool leftSide = true;
    void SpawnEnemy(Enemy e) {
        Vector2 pos = MyHelpers.GetCameraBorderRandomPosition(e.size);
        //if(leftSide) 
        //    pos.x = -horizontalBound - 1;
        //else
        //    pos.x = horizontalBound + 1;
        //pos.y = Random.Range(-verticalBound, verticalBound);
        //leftSide = !leftSide;

        var enemy = Instantiate(e, pos, Quaternion.identity);
        enemy.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyDump").transform);
        spawnedEnemies.Add(enemy);
    }

    IEnumerator SpawnWaveChunk(WaveChunk c) {
        yield return StartCoroutine(MyHelpers.WaitForTime(c.spawnDelay));

        AsteroidSpawner.Instance.SpawnAsteroid();

        for(int i = 0; i < c.count; i++) {
            if(c.isBurst) {
                for(int ii = 0; ii < c.burstNum; ii++) {
                    SpawnEnemy(c.enemy);
                }
            }
            else
                SpawnEnemy(c.enemy);

            //check for game pause
            yield return StartCoroutine(MyHelpers.WaitForTime(1f / c.spawnRate));
        }
    }

    public List<Enemy> GetSpawnedEnemies() {
        return spawnedEnemies;
    }

    public void RemoveEnemy(Enemy e) {
        spawnedEnemies.Remove(e);
    }
}
