using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public Texture2D cursor;

    void Awake() {
        Vector2 hotSpot = new Vector2(cursor.width / 2, cursor.height / 2);
        Cursor.SetCursor(cursor, hotSpot, CursorMode.Auto);
    }
}
