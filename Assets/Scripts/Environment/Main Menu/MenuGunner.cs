using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGunner : MonoBehaviour {

    [HideInInspector]
    public MainManager manager;

    //movement
    [SerializeField] private float moveSpeed, rotateSpeed;

    private float startMoveSpeed;
    private Path path;
    private Vector3 targetPos;
    private int nodeNum = 0;

    //shooting
    [SerializeField] MenuProjectile projectilePrefab;
    [SerializeField] Transform[] fireSpawns;
    [SerializeField] float fireRate, pivotSpeed;
    [SerializeField] bool isAlwaysFire = false;

    float nextFire = 0;
    Transform pivot;
    private MenuEnemy targetEnemy;

    private void Start() {
        startMoveSpeed = moveSpeed;
        pivot = transform.GetChild(0).transform;
        path = FindObjectOfType<PathManager>().ChooseRandomPath();
        //path = FindObjectOfType<PathManager>().ChoosePath(0);
        transform.position = path.GetNode(0).position;
        transform.rotation = path.GetNode(0).rotation;
        pivot.right = path.GetNode(0).right;
        GetNextNode();
        InvokeRepeating(nameof(FindEnemy), 0, Time.fixedDeltaTime);
    }

    private void Update() {
        RotateToNode();
        MoveToNode();
        PivotToTarget();
    }

    #region Movement
    private void RotateToNode() {
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.right = Vector3.Lerp(transform.right, dir, Time.deltaTime * rotateSpeed);
    }

    private void MoveToNode() {
        transform.position += moveSpeed * Time.deltaTime * transform.right;

        if(Vector3.Distance(transform.position, targetPos) <= 0.25f)
            GetNextNode();
    }

    private void GetNextNode() {
        nodeNum++;

        if(nodeNum == 1)
            moveSpeed *= 3;

        else if(nodeNum < path.GetPathSize() - 1)
            moveSpeed = startMoveSpeed;

        else
            moveSpeed *= 3;

        if(nodeNum == path.GetPathSize()) {
            manager.SpawnGunner();
            Destroy(gameObject);
            return;
        }
        targetPos = path.GetNode(nodeNum).position;
    }
    #endregion

    #region Shooting
    private void PivotToTarget() {
        if(!targetEnemy)
            return;

        Vector3 dir = (targetEnemy.transform.position - pivot.position).normalized;
        pivot.right = Vector3.Lerp(pivot.right, dir, Time.deltaTime * pivotSpeed);
        Fire(dir);
    }

    private void Fire(Vector3 direction) {
        nextFire -= Time.deltaTime;

        if(Vector3.Angle(direction, pivot.right) > 7.5f && !isAlwaysFire)
            return;
        if(nextFire > 0)
            return;

        nextFire = 1 / fireRate;
        foreach(var f in fireSpawns) {
            Instantiate(projectilePrefab, f.position, f.rotation);
        }
    }

    void FindEnemy() {
        //find closest enemy
        float closestDistance = 100;
        MenuEnemy closestEnemy = null;

        foreach(var e in manager.enemies) {
            if(e == null)
                continue;

            var d = Vector2.Distance(e.transform.position, transform.position);
            if(d < closestDistance) {
                closestDistance = d;
                closestEnemy = e;
            }
        }

        if(closestEnemy)
            targetEnemy = closestEnemy;
    }
    #endregion
}