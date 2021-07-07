
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int hp;
    public TextMeshProUGUI hpText;

    private void Start() {
        hp = Random.Range(5, 21);
        hpText.text = hp.ToString();
    }

    public void GetDamaged(int damage_) {
        hp -= damage_;
        hpText.text = hp.ToString();

        if(hp <= 0)
            Die();
    }

    private void Die() {
        Destroy(gameObject);
    }
}
