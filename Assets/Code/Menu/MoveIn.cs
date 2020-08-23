using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIn : MonoBehaviour
{

    public int Delay;
    public int Duration;
    public Vector3 From;
    public bool EnablesInteraction;
    public bool useToInstead;
    public Vector3 To;

    RectTransform t;
    Vector3 oldPos;

    void Start() {
        MenuItem.AllowInteraction = false;
        t = GetComponent<RectTransform>();
        oldPos = t.localPosition;
        t.localPosition = From;
        StartCoroutine(Move());
    }

    IEnumerator Move() {
        for (int i = 0; i < Delay; i++)
            yield return null;
        if (useToInstead) {
            oldPos = To;
            From = t.localPosition;
        }
        for (int i = 0; i < Duration; i += 2) {
            float f = i / (float)Duration;
            f *= f;
            f = Mathf.SmoothStep(0, 1, f);
            t.localPosition = (1 - f) * From + f * oldPos;
            yield return null;
            yield return null;
        }
        t.localPosition = oldPos;
        if (EnablesInteraction) {
            MenuItem.AllowInteraction = true;
            GameData.Paused = false;
        }
    }
}
