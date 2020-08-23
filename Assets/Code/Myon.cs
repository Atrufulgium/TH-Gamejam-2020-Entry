using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Myon : MonoBehaviour
{
    public Transform t;

    public static Myon MyonMyon;

    Unity.Mathematics.Random rng;

    void Start() {
        MyonMyon = this;
        t = GetComponent<Transform>();
        rng = new Unity.Mathematics.Random();
        rng.InitState();
        StartCoroutine(DoMovement());
    }

    IEnumerator DoMovement() {
        while (Movement.Player == null || GameData.Paused)
            yield return null;
        Vector3 prevTarget = t.position;
        while (true) {
            Vector3 target = (Vector2)Movement.Player.t.position + (Vector2)rng.NextFloat2Direction() * 0.5f;
            target.z = 5;
            for (int i = 0; i < 60; i++) {
                Vector3 pos = t.position;
                t.position = 97.5f * pos / 100f + target * 1.5f / 100f + prevTarget / 100f;
                while (GameData.Paused)
                    yield return null;
                yield return null;
            }
            prevTarget = target;
        }
    }

    private void Update() {
        if (GameData.Paused)
            return;
        if (GameData.PlayerDied) {
            StopAllCoroutines();
            GetComponent<EverythingMoves>().enabled = true;
            StartCoroutine(Rip());
        }
    }

    IEnumerator Rip() {
        while (true) {
            while (GameData.Paused)
                yield return null;
            t.localScale -= Vector3.one * 0.01f;
            if (t.localScale.x < 0.05)
                ActualRip();
            yield return null;
        }
    }

    void ActualRip() {
        Destroy(gameObject);
    }
}
