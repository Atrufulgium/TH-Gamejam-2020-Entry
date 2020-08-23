using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour {

    RectTransform t;

    private void Awake() {
        t = GetComponent<RectTransform>();
    }

    public void Move(float y) {
        Vector3 p = t.localPosition;
        p.y = y;
        StartCoroutine(SelectMove(p));
    }

    IEnumerator SelectMove(Vector3 targetPos) {
        Vector3 currentPos = t.localPosition;
        for (int i = 0; i < 10; i++) {
            float f = i / 10f;
            f = Mathf.SmoothStep(0, 1, f);
            t.localPosition = (1 - f) * currentPos + f * targetPos;
            yield return null;
        }
        t.localPosition = targetPos;
    }

}