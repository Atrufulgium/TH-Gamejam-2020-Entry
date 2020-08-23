using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    Transform t;
    int time = 0;

    void Start() {
        t = GetComponent<Transform>();
    }
    
    void Update() {
        time++;
        t.rotation = Quaternion.Euler(0, 0, Mathf.Sin(time * 0.04f) * 20);
    }
}
