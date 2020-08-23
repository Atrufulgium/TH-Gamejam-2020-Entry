using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MenuDoer))]
public class MenuItem : MonoBehaviour {

    public bool Selected = false;

    RectTransform t;
    MenuDoer[] m;
    Vector3 initPos;

    public MenuItem Root;
    public MenuItem Next;
    public MenuItem Prev;
    // todo: breadcrumbs

    public static bool AllowInteraction = true;
    static int Cooldown = 0;
    static int MaxCooldown = 30;
    static int Combo = 2;

    void Start() {
        t = GetComponent<RectTransform>();
        m = GetComponents<MenuDoer>();
        initPos = t.localPosition;
        if (Selected)
            Select();
    }

    void Update() {
        if (!AllowInteraction)
            return;
        if (Selected) {
            if (Cooldown <= 0) {
                if (Input.GetKey(GameData.Down) && Next != null) {
                    Cooldown = MaxCooldown / (Combo / 2);
                    if (Combo < 20)
                        Combo++;
                    AudioManager.PlaySFX(SFX.Confirm);
                    Next.Select();
                    Deselect();
                }
                if (Input.GetKey(GameData.Jump) && Prev != null) {
                    Cooldown = MaxCooldown / (Combo / 2);
                    if (Combo < 20)
                        Combo++;
                    AudioManager.PlaySFX(SFX.Confirm);
                    Prev.Select();
                    Deselect();
                }
                if (Input.GetKey(GameData.Confirm) && m.Any(m => m.Confirmable)) {
                    Cooldown = MaxCooldown / (Combo / 2);
                    if (Combo < 20)
                        Combo++;
                    AudioManager.PlaySFX(SFX.Confirm);
                    ConfirmSomething();
                }
                if (Input.GetKey(GameData.Cancel)) {
                    if (m.Any(m => m.Cancelable)) {
                        Cooldown = MaxCooldown / Combo;
                        AudioManager.PlaySFX(SFX.Cancel);
                        foreach (MenuDoer d in m)
                            d.Cancel();
                    } else if (Root != null) {
                        Cooldown = MaxCooldown / Combo;
                        AudioManager.PlaySFX(SFX.Cancel);
                        Root.Select();
                        Deselect();
                    }
                }
                if (Input.GetKey(GameData.Cancel) && gameObject.name == "Quit") {
                    AudioManager.PlaySFX(SFX.Cancel);
                    ConfirmSomething();
                }
            } else {
                Cooldown--;
                if (Input.GetKeyUp(GameData.Down) || Input.GetKeyUp(GameData.Jump)
                    || Input.GetKeyUp(GameData.Confirm) || Input.GetKeyUp(GameData.Cancel)) {
                    Cooldown = 2;
                    Combo = 2;
                }
            }

        }
    }

    public void Select() {
        foreach (MenuDoer d in m)
            d.Select();
        Selected = true;
        StopAllCoroutines();
        StartCoroutine(SelectMove());
    }

    public void Deselect() {
        foreach (MenuDoer d in m)
            d.Deselect();
        Selected = false;
        StopAllCoroutines();
        StartCoroutine(DeselectMove());
    }

    IEnumerator SelectMove() {
        Vector3 currentPos = t.localPosition;
        Vector3 targetPos = initPos - new Vector3(50, 0, 0);
        for (int i = 0; i < 10; i++) {
            float f = i / 10f;
            t.localPosition = (1 - f) * currentPos + f * targetPos;
            yield return null;
        }
        t.localPosition = targetPos;
    }

    IEnumerator DeselectMove() {
        Vector3 currentPos = t.localPosition;
        for (int i = 0; i < 10; i++) {
            float f = i / 10f;
            t.localPosition = (1 - f) * currentPos + f * initPos;
            yield return null;
        }
        t.localPosition = initPos;
    }

    void ConfirmSomething() {
        foreach (MenuDoer d in m)
            d.Confirm();
    }
}
