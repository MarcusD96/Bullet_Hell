
using UnityEngine;

public class PathManager : MonoBehaviour {

    [SerializeField] Path[] playerPaths;
    [SerializeField] Path[] enemyPaths;

    public Path ChooseRandomPath() {
        return playerPaths[Random.Range(0, playerPaths.Length)];
    }

    public Path ChoosePath(int index) {
        return playerPaths[index];
    }

    public Path ChooseRandomEnemyPath() {
        return enemyPaths[Random.Range(0, enemyPaths.Length)];
    }

    public Path ChooseEnemyPath(int index) {
        return enemyPaths[index];
    }
}
