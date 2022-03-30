
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

    #region Singleton
    public static AsteroidSpawner Instance;

    private void Awake() {
        if(Instance) {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    #endregion

    [SerializeField] Asteroid asteroidPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] List<Asteroid> spawnedAsteroids = new List<Asteroid>();

    public Asteroid SpawnAsteroid() {
        var s = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var a = Instantiate(asteroidPrefab, s.position, s.rotation);
        spawnedAsteroids.Add(a);
        a.transform.SetParent(transform);
        a.startHP = LevelStats.Level * 10;
        return a;
    }

    public List<Asteroid> GetAsteroids() {
        return spawnedAsteroids;
    }

}
