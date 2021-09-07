
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Player gunner;

    void Awake() {
        Instantiate(gunner.gameObject, Vector2.zero, Quaternion.identity);
    }
}
