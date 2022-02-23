
using UnityEngine;

public class BossEnemy : Enemy {

    public UpgradeStar upgradeStar;
    public Enemy babySpawns;
    public float numSpawn;

    protected override void Die() {
        if(!isDead) {
            isDead = true;
            SpawnXP();
            Instantiate(upgradeStar, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            Vector2 pos = transform.position;
            for(int i = 0; i < numSpawn; i++) {
                EnemySpawner.Instance.GetSpawnedEnemies().Add(Instantiate(babySpawns, new Vector3(Random.Range(pos.x - 1, pos.x + 1), Random.Range(pos.y - 1, pos.y + 1)), Quaternion.identity));
            }
            EnemySpawner.Instance.RemoveEnemy(this);
            Destroy(gameObject); 
        }
    }
}
