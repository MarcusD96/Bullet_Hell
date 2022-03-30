using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public float moveSpeed, rotateSpeed;

    [SerializeField]
    private Moon[] moonPrefabs;

    private int maxNumMoons = 5;

    private void Start() {
        if(Random.Range(0, 2) < 1)
            rotateSpeed = -rotateSpeed;

        InitializeMoons();
    }

    // Update is called once per frame
    void Update() {
        transform.position += moveSpeed * Time.deltaTime * Vector3.left;
        transform.Rotate(Vector3.forward * rotateSpeed);

        if(transform.position.x <= -15)
            Destroy(gameObject);
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
}
