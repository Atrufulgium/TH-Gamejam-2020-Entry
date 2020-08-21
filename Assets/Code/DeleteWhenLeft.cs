using System.Collections;
using UnityEngine;

public class DeleteWhenLeft : MonoBehaviour {

    Transform t;
    SpriteRenderer r;

    void Start() {
        t = GetComponent<Transform>();
        r = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (r.bounds.max.x < GameData.FieldL) {
            Destroy(gameObject);
        }
    }
}
