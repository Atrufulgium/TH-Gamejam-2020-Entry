using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static long PlayerScore { get; private set; } = 0;
    static Score s;

    long visibleScore = 0;
    bool scoreDirty = false;

    RectTransform rt;
    Text t;

    void Start() {
        s = this;
        rt = GetComponent<RectTransform>();
        t = GetComponent<Text>();
        StartCoroutine(GrowShrink());
    }

    private void LateUpdate() {
        if (scoreDirty) {
            t.text = $"<size=60>score</size> {visibleScore.ToString("### ### ### ### ### ### ### ##0").Trim()}";
            scoreDirty = false;
        }
    }

    public static void AddScore(long amount) {
        s.addScore(amount);
    }

    void addScore(long amount) {
        // Make sure it's a nice round number.
        amount = (amount / 50) * 50;
        PlayerScore += amount;
        StartCoroutine(AddScoreOverTime(amount));
    }

    IEnumerator AddScoreOverTime(long amount) {
        long delta = amount / 50;
        for (int i = 0; i < 50; i++) {
            visibleScore += delta;
            scoreDirty = true;
            yield return null;
        }
    }

    IEnumerator GrowShrink() {
        while (true) {
            if (scoreDirty) {
                for (int i = 0; i < 50; i += 2) {
                    rt.localScale = Vector3.one * (1 + (1 - Mathf.Cos(i / 25f * 2 * Mathf.PI)) / 20f);
                    yield return null;
                    yield return null;
                }
            } else {
                yield return null;
            }
        }
    }
}
