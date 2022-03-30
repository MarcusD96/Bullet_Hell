using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {
    [SerializeField] MenuGunner[] gunners;
    [SerializeField] MenuEnemy enemy;

    public List<MenuEnemy> enemies = new List<MenuEnemy>();

    private void Start() {
        InvokeRepeating(nameof(SpawnEnemy), 0, 3);
        SpawnGunner();
    }

    void SpawnEnemy() {
        var e = Instantiate(enemy, transform);
        e.manager = this;
        enemies.Add(e);
    }

    public void SpawnGunner() {
        var g = Instantiate(gunners[Random.Range(0, gunners.Length)], transform);
        //var g = Instantiate(gunners[0], transform);
        g.manager = this;
    }

}
