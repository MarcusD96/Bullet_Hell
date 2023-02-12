using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public float moveSpeed, rotateSpeed;

    [SerializeField]
    private Moon[] moonPrefabs;

    private int maxNumMoons = 3;
    private Vector3 direction;

    private void Start() {
        if(Random.Range(0, 2) < 1)
            rotateSpeed = -rotateSpeed;

        direction = (Vector3.zero - transform.position).normalized;
        transform.position -= direction * 10f;

        moveSpeed += Random.Range(-0.025f, 0.025f);

        //InitializeMoons();
    }

    // Update is called once per frame
    void Update() {
        if(PauseMenu.Instance)
            if(PauseMenu.Instance.isPaused)
                return;

        transform.position += moveSpeed * Time.deltaTime * direction;
        transform.Rotate(Vector3.forward * rotateSpeed);
    }

    void InitializeMoons() {
        int r = Random.Range(1, maxNumMoons + 1);
        float rot = 360f / (float) r;

        for(int i = 0; i < r; i++) {
            var m = Instantiate(moonPrefabs[Random.Range(0, moonPrefabs.Length)], transform);
            m.transform.rotation = Quaternion.Euler(0, 0, rot);
            rot += rot;
        }
    }

    public void SetDirection(Vector3 newDirection_) => direction = newDirection_;
}
