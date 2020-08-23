using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// imagine efficiency
public class EverythingMoves : MonoBehaviour {

    public static float MoveSpeed = GameData.DefaultSpeed;
    public float Speed = -1f;
    public float ParallaxFactor = 1f;

    private Transform t;

    void Start() {
        t = GetComponent<Transform>();
        if (Speed != -1f)
            SetSpeed();
    }
    
    void Update() {
        if (GameData.Paused)
            return;
        t.position += Vector3.left * MoveSpeed * ParallaxFactor;
    }

    private void OnValidate() {
        if (Speed != -1f)
            SetSpeed();
    }

    void SetSpeed() {
        MoveSpeed = Speed / 60f;
        Debug.Log($"Gameobject {name} changed the speed to {MoveSpeed} tiles/s");
    }
}
