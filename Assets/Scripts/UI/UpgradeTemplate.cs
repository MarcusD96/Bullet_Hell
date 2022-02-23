
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTemplate : MonoBehaviour {

    public TextMeshProUGUI title;
    public Image sprite;
    public TextMeshProUGUI description;

    public void SetUpgrade(Upgrade u) {
        title.text = u.title;
        sprite.sprite = u.sprite;
        description.text = u.description;
    }
}
