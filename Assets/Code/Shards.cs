using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shards : MonoBehaviour {

    public static Stack<Shards> shards = new Stack<Shards>();
    public bool pooled = true;
    SpriteRenderer s;
    Transform t;
    float angularVelocity;
    Vector3 velocity;
    EverythingMoves m;

    void Start() {
        m = GetComponent<EverythingMoves>();
        m.enabled = false;
        if (pooled)
            shards.Push(this);
        s = GetComponent<SpriteRenderer>();
        t = GetComponent<Transform>();
    }
    
    void Update() {
        if (!pooled) {
            if (t.position.sqrMagnitude < 150) {
                t.Rotate(0, 0, angularVelocity);
                t.position += velocity;
                velocity.y -= 0.0016f;
            } else {
                transform.position = new Vector3(-10, 0, 2);
                shards.Push(this);
                pooled = true;
                m.enabled = false;
            }
        }
    }

    public static void ExplodeAt(Vector2 pos, Color c, int count, float force = 1) {
        for (int i = 0; i < count; i++) {
            if (shards.Count == 0)
                return;
            Shards s = shards.Pop();
            s.t.position = new Vector3(pos.x, pos.y, 2);
            s.t.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            s.angularVelocity = Random.Range(0.5f, 1f + force) * (Random.Range(0,2) * 2 - 1);
            s.velocity = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0) * force / 60f;
            s.s.color = c;
            s.m.enabled = true;
            s.pooled = false;
        }
    }

    public static void Generate(int count) {
        GameObject obj = Resources.Load<GameObject>("Prefabs/Shards");
        for (int i = 0; i < count; i++) {
            Instantiate(obj);
        }
    }
}
