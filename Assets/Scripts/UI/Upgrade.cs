﻿
using UnityEngine;

[System.Serializable]
public class Upgrade : MonoBehaviour {

    public Player upgradePrefab;
    public string title;
    public Sprite sprite;
    public string description;
    public Upgrade[] childUpgrades;
}
