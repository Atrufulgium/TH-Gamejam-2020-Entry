using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuDoer : MonoBehaviour {
    public abstract bool Confirmable { get; }
    public abstract bool Cancelable { get; }

    protected RectTransform t;
    protected MenuItem m;

    private void Awake() {
        t = GetComponent<RectTransform>();
        m = GetComponent<MenuItem>();
    }

    public virtual void Confirm() { }
    public virtual void Cancel() { }
    public virtual void Select() { }
    public virtual void Deselect() { }
}
