using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ComboText : MonoBehaviour {

    public static Dictionary<string, ComboText> ComboTexts = new Dictionary<string, ComboText>();
    public int Duration;
    public bool Active => coroutineCount > 0;
    new public string name;

    Color c;
    RectTransform rt;
    Text t;
    List<float> sizes;
    int coroutineCount = 0;

    bool stoppingCombo = false;

    void Awake() {
        rt = GetComponent<RectTransform>();
        t = GetComponent<Text>();
        c = t.color;
        if (ComboTexts.ContainsKey(name))
            ComboTexts[name] = this;
        else
            ComboTexts.Add(name, this);

        sizes = new List<float>();
    }

    private void Update() {
        sizes.Clear();
    }

    private void LateUpdate() {
        if (sizes.Count > 0) {
            float max = sizes.Max();
            c.a = (max - 1f) * 10f;
            t.color = c;
            rt.localScale = new Vector3(max, max, 1);
        }
    }

    public void CreateComboText(int num) {
        if (stoppingCombo)
            return;
        // could do fancy with a stringbuilder but lol
        t.text = $"<size=80>x</size>{num} Combo!";
        StartCoroutine(doTheComboThing());
    }

    public void StopCombo() {
        if (stoppingCombo || !Active)
            return;
        stoppingCombo = true;
        StopAllCoroutines();
        coroutineCount = 0;
        sizes.Clear();
        StartCoroutine(ripTheComboThing());
    }

    private IEnumerator doTheComboThing() {
        coroutineCount++;
        int progress = 0;
        float scale = 1f;
        for (int i = 0; i < Duration; i++) {
            float f = progress / (float)Duration;
            f *= f;
            if (f < 0.1f)
                scale = Mathf.SmoothStep(1, 1.1f, f * 10);
            else if (f > 0.5f)
                scale = Mathf.SmoothStep(1.1f, 1, (f - 0.5f) * 2);
            sizes.Add(scale);
            progress++;
            yield return null;
        }
        sizes.Add(1);
        coroutineCount--;
    }

    private IEnumerator ripTheComboThing() {
        Color oldCol = c;
        float oldAlpha = oldCol.a;
        float oldRot = rt.rotation.eulerAngles.z;
        float oldScale = rt.localScale.x;
        int progress = 0;
        Color col = Color.red;
        t.color = col;
        for (int i = 0; i < 60; i++) {
            float f = progress / 60f;
            rt.localScale = new Vector3(1 + col.a / 10f, 1 + col.a / 10f, 1);
            if (f < 0.33f) {
                rt.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(oldRot, 120, f * 3f));
                col.a = Mathf.SmoothStep(oldAlpha, 1, f * 3);
            } else {
                rt.rotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(120, 65, (f - 0.667f) * 3f));
                col.a = Mathf.SmoothStep(1, 0, (f - 0.667f) * 3);
            }
            t.color = col;
            progress++;
            yield return null;
        }
        oldCol.a = 0;
        t.color = oldCol;
        rt.localScale = Vector3.one;
        rt.rotation = Quaternion.Euler(0, 0, oldRot);
        stoppingCombo = false;
    }
}
