using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave {
    public string name;
    public WaveChunk[] chunks;

    public float GetChunkTime() {
        float time = 0;

        float longestTime = 0;
        foreach(var c in chunks) {
            float t = c.spawnDelay + (c.count * (1 / c.spawnRate));
            if(t > longestTime)
                time = t;
        }

        time += 5;
        return time;
    }
}

[System.Serializable]
public class WaveChunk {
    public Enemy enemy;
    public int count;
    public float spawnRate, spawnDelay;
}
