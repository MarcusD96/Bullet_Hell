
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour {

    [SerializeField] Image sprite;
    [SerializeField] float useRate = 0;

    protected bool isUsingAbility = false;

    float nextUse = 0f;

    private void Update() {
        if(PauseMenu.Instance.isPaused)
            return;

        UpdateSprite();

        if(isUsingAbility == true)
            return;

        nextUse -= Time.deltaTime;

        if(nextUse > 0)
            return;

        Input();
    }

    void Input() {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Space)) {
            OnActivate();
            nextUse = useRate;
        }
    }

    void UpdateSprite() {
        sprite.fillAmount = nextUse / useRate;
    }

    protected virtual void OnActivate() {
        Debug.Log("TODO: avtivate ability " + GetComponent<Ability>().ToString());
    }
}
