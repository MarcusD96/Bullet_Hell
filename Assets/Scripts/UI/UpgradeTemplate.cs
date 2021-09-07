
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTemplate : MonoBehaviour {

    public TextMeshProUGUI title, cost;
    public Image sprite;
    public TextMeshProUGUI description;

    public void SetUpgrade(Upgrade u) {
        title.text = u.title;
        cost.text = u.cost.ToString();
        sprite.sprite = u.sprite;
        description.text = u.description;
        if(PlayerStatsManager.Instance.GetLevel() < u.cost)
            GetComponent<Button>().interactable = false;
    }
}
