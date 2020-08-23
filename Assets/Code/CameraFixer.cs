using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFixer : MonoBehaviour {

    static CameraFixer f;
    float previousRatio = 9 / 16f;
    new Camera camera;

    private void Awake() {
        f = this;
        camera = GetComponent<Camera>();
    }
    
    private void Update() {
        // https://forum.unity.com/threads/force-camera-aspect-ratio-16-9-in-viewport.385541/
        float ratio = Screen.height / (float)Screen.width;
        if (Mathf.Abs(ratio - previousRatio) > 0.00001f) {
            camera.orthographicSize = ratio > 9 / 16f ? ratio * 9.6f : 5.4f;
            previousRatio = ratio;
        }
    }

    public static void Screenshake() {
        f.StartCoroutine(f.screenShake());
    }

    private IEnumerator screenShake() {
        // https://medium.com/@mattThousand/basic-2d-screen-shake-in-unity-9c27b56b516
        Vector3 initPos = camera.transform.position;
        for (int i = 0; i < 20; i += 2) {
            while (GameData.Paused)
                yield return null;
            camera.transform.position = initPos + (Vector3)(Vector2)Random.onUnitSphere / Mathf.Pow(i+1, 0.8f);
            yield return null;
            yield return null;
        }
        camera.transform.position = initPos;
    }
}