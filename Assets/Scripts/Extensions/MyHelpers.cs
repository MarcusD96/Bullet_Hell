
using System.Collections;
using UnityEngine;

public static class MyHelpers {

    public static Quaternion RandomZRotation {
        get {
            return Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }

    public static Vector3 RandomXYScale(float min_, float max_) {
        float r = Random.Range(min_, max_);
        return new Vector3(r, r, 1);
    }

    public static IEnumerator WaitForTime(float time_) {
        float endTime = time_;
        float s = 0;
        while(s < endTime) {
            if(PauseMenu.Instance)
                if(PauseMenu.Instance.isPaused) {
                    yield return null;
                    continue;
                }
            s += Time.deltaTime;
            yield return null;
        }
    }

    public static Vector2 GetCameraBorderRandomPosition(float offset_) {
        offset_ *= 2f;

        Vector2 position;
        Vector2 camExtents = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector2 neg = new Vector2(-camExtents.x, -camExtents.y);
        Vector2 pos = new Vector2(camExtents.x, camExtents.y);

        //choose (x, y) top/right or bottom/left
        int randCorner = Random.Range(0, 2);
        if(randCorner == 0)
            position = pos;
        else
            position = neg;

        //choose x or y
        //choose random number between positive and negative chosen dimension
        int randDim = Random.Range(0, 2);
        //spawn at top/bottom
        if(randDim == 0) {
            position.x = Random.Range(-position.x, position.x);
            if(pos.y < 0)
                position.y -= offset_;
            else
                position.y += offset_;
        }
        //spawn on sides
        else {
            position.y = Random.Range(-position.y, position.y);
            if(pos.x < 0)
                position.x -= offset_;
            else
                position.x += offset_;
        }

        return position;
    }

    public static Vector2 MoveTowards(Vector2 pos1, Vector2 pos2, float step) {
        Vector2 pos = pos2 - pos1;
        float mag = Vector2.Dot(pos, pos);

        if(mag < step * step)
            return pos2;

        Vector2 dir = pos / Mathf.Sqrt(mag);

        return pos1 + (step * dir);
    }

    public static Vector3 GetRandomPointOnScreen(float size_) {
        Rect camRect = Camera.main.rect;

        float x = camRect.x;
        float y = camRect.y;

        float randX = Random.Range(-x + size_, x - size_);
        float randY = Random.Range(-y + size_, y - size_);

        return new Vector3(randX, randY);
    }

    public static Vector2 VaryDirection(Vector2 directionVector, float variation_) {
        float rX = Random.Range(-variation_, variation_);
        float rY = Random.Range(-variation_, variation_);
        Vector2 dir = new Vector2(directionVector.x + rX, directionVector.y + rY);
        dir.Normalize();
        return dir;
    }
}
