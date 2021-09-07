using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy {

    public Enemy babySpawns;
    public float numSpawn;

    protected override void Die(bool awardPoints) {
        if(!isDead) {
            isDead = true;
            PlayerStatsManager.Instance.AddXP(xpWorth);
            Vector2 pos = transform.position;
            for(int i = 0; i < numSpawn; i++) {
                EnemySpawner.Instance.GetSpawnedEnemies().Add(Instantiate(babySpawns, new Vector3(Random.Range(pos.x - 1, pos.x + 1), Random.Range(pos.y - 1, pos.y + 1)), Quaternion.identity));
            }
            Destroy(gameObject); 
        }
    }
}
