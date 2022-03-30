using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnemy : MonoBehaviour {

    [HideInInspector]
    public MainManager manager;

    public float radius;

    //movement
    [SerializeField] private float moveSpeed, rotateSpeed;
    private Path path;
    private Vector3 target;
    private int nodeNum = 1;

    private void Start() {
        path = FindObjectOfType<PathManager>().ChooseRandomEnemyPath();
        transform.position = path.GetNode(0).position;
        transform.rotation = path.GetNode(0).rotation;
        target = path.GetNode(nodeNum).position;
    }

    private void Update() {
        RotateToNode();
        MoveToNode();
    }

    #region Movement
    private void RotateToNode() {
        Vector3 dir = (target - transform.position).normalized;
        transform.right = Vector3.Lerp(transform.right, dir, Time.deltaTime * rotateSpeed);
    }

    private void MoveToNode() {
        transform.position += moveSpeed * Time.deltaTime * transform.right;

        if(Vector3.Distance(transform.position, target) <= 1f)
            GetNextNode();
    }

    private void GetNextNode() {
        nodeNum++;
        if(nodeNum == path.GetPathSize()) {
            Die();
            return;
        }
        target = path.GetNode(nodeNum).position;
    }
    #endregion

    public void Hit() {
        Die();
    }

    void Die() {
        manager.enemies.Remove(this);
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
