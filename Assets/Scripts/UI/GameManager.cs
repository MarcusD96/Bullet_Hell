
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Singleton
    public static GameManager Instance;
    public bool IsStarted { get; private set; } = false;

    private void Awake() {
        if(Instance) {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    #endregion

    public Player gunner, defaultGunner;
    public Upgrade choice, defaultChoice;
    public bool useController = false;

    private void Start() {
        DontDestroyOnLoad(this);
    }

    public bool ToggleController() {
        useController = !useController;
        return useController;
    }

    public void SpawnGunner() {
        if(!gunner) {
            choice = defaultChoice;
            gunner = defaultGunner;
        }

        var p = Instantiate(gunner, Vector2.zero, Quaternion.identity);
        UpgradeManager.Instance.CurrentGunner = p;
        StatsUI.Instance.SetNewPlayer(p);
        FindObjectOfType<EnemySpawner>().StartSpawning();
        IsStarted = true;
    }
}
