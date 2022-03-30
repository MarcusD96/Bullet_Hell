
using System.Collections;
using UnityEngine;

public static class MyHelpers {

    public static Quaternion RandomZRotation {
        get {
            return Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }

    public static IEnumerator WaitForTime(float time_) {
        float endTime = time_;
        float s = 0;
        while(s < endTime) {
            if(PauseMenu.Instance.isPaused) {
                yield return null;
                continue;
            }
            s += Time.deltaTime;
            yield return null;
        }
    }

}
