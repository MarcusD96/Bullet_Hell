using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyLevel : MonoBehaviour {

    TextMeshProUGUI text;

    void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate() {
        text.text = LevelStats.Level.ToString();
    }
}
