using System.Collections;
using UnityEngine;

// Can just assume everything exits left
public class WrapAround : MonoBehaviour {

    // in ticks because ticks > seconds
    public int Delay = 0;
    public float overrideBoundSize = -1f;
    public bool idontcare = false;

    Transform t;
    SpriteRenderer r;

    float extra;
    public bool warping = false;

    void Start() {
        t = GetComponent<Transform>();
        r = GetComponent<SpriteRenderer>();
        extra = overrideBoundSize;
        if (extra == -1f)
            extra = r.bounds.size.x;
    }
    
    void Update() {
        if (!warping && r.bounds.min.x + extra < GameData.FieldL) {
            StartCoroutine(Warp());
        }
    }

    IEnumerator Warp() {
        warping = true;
        Vector3 p = t.position;
        for (int i = 0; i < Delay; i++) {
            yield return null;
        }
        p.x += GameData.FieldWidth + extra;
        if (idontcare)
            p.x += extra;
        t.position = p;
        yield return null; //not a gamejam if no shitty hacky solutions
        warping = false;
    }
}
