using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlay : MenuDoer {
    public RectTransform Menu;
    public string Level;
    public override bool Confirmable => true;
    public override bool Cancelable => false;
    public override void Confirm() {
        m.Selected = false;
        StartCoroutine(enumerator());
    }

    IEnumerator enumerator() {
        Vector3 oldPos = Menu.localPosition;
        Vector3 targetPos = oldPos;
        targetPos.x = 1850;
        for (int i = 0; i < 60; i++) {
            float f = i / 60f;
            f *= f;
            f = Mathf.SmoothStep(0, 1, f);
            Menu.localPosition = targetPos * f + (1 - f) * oldPos;
            yield return null;
        }
        SceneManager.LoadScene(Level);
    }
}