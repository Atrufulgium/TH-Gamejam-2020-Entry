using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WrapAround), typeof(EverythingMoves))]
public class Deco : MonoBehaviour {

    public Sprite[] pool;

    Transform t;
    SpriteRenderer r;
    WrapAround w;

    bool previousWarp = false;
    
    void Start() {
        t = GetComponent<Transform>();
        w = GetComponent<WrapAround>();
        r = GetComponent<SpriteRenderer>();
        Select();
    }

    private void Update() {
        if (w.warping && !previousWarp) {
            Select();
            w.Delay = Random.Range(0, 30);
        }
        previousWarp = w.warping;
    }

    public void Select() {
        r.sprite = pool[Random.Range(0, pool.Length)];
        t.localScale = new Vector3(Random.Range(0, 2) * 2 - 1, 1, 1);
    }
}
