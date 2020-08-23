using UnityEngine;

public class MenuToggleObject : MenuDoer {
    public GameObject target;
    public override bool Confirmable => false;
    public override bool Cancelable => false;
    public override void Confirm() { }
    public override void Select() {
        target.SetActive(true);
    }
    public override void Deselect() {
        target.SetActive(false);
    }
}