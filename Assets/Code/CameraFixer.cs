using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFixer : MonoBehaviour {

    float previousRatio = 9 / 16f;
    new Camera camera;

    private void Awake() {
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
}