using UnityEngine;

public class MenuQuit : MenuDoer {
    public override bool Confirmable => true;
    public override bool Cancelable => true;
    public override void Confirm() {
        Debug.Log("Quitting!");
        Application.Quit(0);
    }

    public override void Cancel() {
        Confirm();
    }
}