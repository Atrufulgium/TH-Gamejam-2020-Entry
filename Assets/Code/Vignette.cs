using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vignette : MonoBehaviour {

    public static Dictionary<string, Vignette> Vignettes = new Dictionary<string, Vignette>();

    Color c;
    List<float> alphas = new List<float>();
    
    public int Duration;
    new public string name;

    SpriteRenderer r;
    void Awake() {
        if (Vignettes.ContainsKey(name))
            Vignettes[name] = this;
        else
            Vignettes.Add(name, this);
        r = GetComponent<SpriteRenderer>();
        c = r.color;
    }

    private void Update() {
        alphas.Clear();
    }

    private void LateUpdate() {
        if (alphas.Count > 0) {
            float max = alphas.Sum();
            c.a = Mathf.Clamp01(max);
            r.color = c;
        }
    }

    public void Flash() {
        StartCoroutine(doTheFlashyThing());
    }

    private IEnumerator doTheFlashyThing() {
        int progress = 0;
        float alpha = 0f;
        for (int i = 0; i < Duration; i++) {
            float f = progress / (float)Duration;
            f *= f;
            if (f < 0.25f)
                alpha = Mathf.SmoothStep(0, 1, f * 4) * 0.3f;
            else
                alpha = Mathf.SmoothStep(1, 0, (f - 0.25f) * 4/3f) * 0.3f;
            alphas.Add(alpha);
            progress++;
            yield return null;
        }
        alphas.Add(0);
    }
}
