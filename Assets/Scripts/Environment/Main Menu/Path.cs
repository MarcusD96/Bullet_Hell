using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Path : MonoBehaviour {
    [SerializeField] Colors color;

    Transform[] nodes;

    private void Start() {
        nodes = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) {
            nodes[i] = transform.GetChild(i);
        }
    }

    private void LateUpdate() {
#if UNITY_EDITOR
        nodes = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) {
            nodes[i] = transform.GetChild(i);
        }
        nodes[0].right = (nodes[1].position - nodes[0].position).normalized;
#endif
    }

    public int GetPathSize() {
        return nodes.Length;
    }

    public Transform GetNode(int index) {
        return nodes[index];
    }

    private void OnDrawGizmosSelected() {

        switch(color) {
            case Colors.YELLOW:
                Gizmos.color = Color.yellow;
                break;
            case Colors.BLUE:
                Gizmos.color = Color.blue;
                break;
            case Colors.GREEN:
                Gizmos.color = Color.green;
                break;
            case Colors.WHITE:
                Gizmos.color = Color.white;
                break;
            case Colors.LIGHT:
                Gizmos.color = new Color(1, 1f / 255f * 170f, 1f / 255f * 170f);
                break;
            case Colors.PINK:
                Gizmos.color = new Color(1, 1f / 255f * 120f, 1f / 255f * 120f);
                break;
            case Colors.RED:
                Gizmos.color = Color.red;
                break;
            default:
                break;
        }

        for(int i = 0; i < nodes.Length; i++) {
            Gizmos.DrawSphere(nodes[i].position, 0.25f);
            if(i < nodes.Length - 1)
                Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
        }
    }
}

public enum Colors {
    YELLOW,
    BLUE,
    GREEN,
    WHITE,
    LIGHT,
    PINK,
    RED
}
