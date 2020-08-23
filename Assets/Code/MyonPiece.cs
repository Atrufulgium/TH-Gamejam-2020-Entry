using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyonPiece : MonoBehaviour {

    Transform t;

    bool shouldRip = false;

    void Start() {
        t = GetComponent<Transform>();
    }
    
    void Update() {
        if (GameData.Paused)
            return;
        t.localScale -= Vector3.one * 0.01f;
        if (GameData.PlayerDied)
            shouldRip = true;
        if (t.localScale.x < 0.05) {
            if (shouldRip)
                Destroy(gameObject);
            else {
                t.position = Myon.MyonMyon.t.position;
                t.localScale = Vector3.one;
            }
        }
    }
}
