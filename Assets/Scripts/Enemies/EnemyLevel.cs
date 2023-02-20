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
        if(PauseMenu.Instance.isPaused)
            return;

        text.text = (EnemySpawner.Instance.waveNumber).ToString();
    }
}
