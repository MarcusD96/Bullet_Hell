
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Player gunner;
    public bool useController = false;

    void Start() {
        Instantiate(gunner, Vector2.zero, Quaternion.identity);
    }
}
