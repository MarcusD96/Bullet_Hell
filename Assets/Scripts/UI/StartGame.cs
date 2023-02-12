using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    [SerializeField] Button button;

    private void Start() {
        if(!GameManager.Instance.useController)
            Cursor.visible = true;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public void StartGame_() {
        GameManager.Instance.SpawnGunner();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        AudioManager.Instance.PlaySound("Menu Click");
        Destroy(gameObject);
    }

}
