using UnityEngine.SceneManagement;

public class MenuGoTo : MenuDoer {
    public Mover Mover;
    public MenuItem SelectTarget;
    public float y;
    public override bool Confirmable => true;
    public override bool Cancelable => true;
    public override void Confirm() {
        m.Deselect();
        SelectTarget.Select();
        Mover.Move(y);
    }
    public override void Cancel() {
        Confirm();
    }
}