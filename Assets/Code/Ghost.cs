using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction { Wolf, Otter, Eagle };

public class Ghost : MonoBehaviour {

    public static int OtterCombo = 0;
    public static int EagleCombo = 0;
    public Faction faction;

    static int KatanaLayer = -1;
    int time = 0;
    Transform t;

    Vignette WolfV;
    Vignette OtterV;
    Vignette EagleV;

    ComboText OtterC;
    ComboText EagleC;

    void Start() {
        WolfV = Vignette.Vignettes["Wolf"];
        OtterV = Vignette.Vignettes["Otter"];
        EagleV = Vignette.Vignettes["Eagle"];
        OtterC = ComboText.ComboTexts["Otter"];
        EagleC = ComboText.ComboTexts["Eagle"];
        t = GetComponent<Transform>();
        if (KatanaLayer == -1)
            KatanaLayer = LayerMask.NameToLayer("katana");
    }
    
    void Update() {
        if (GameData.Paused)
            return;
        time++;
        t.rotation = Quaternion.Euler(0, 0, Mathf.Sin(time * 0.04f) * 20);
        if (GameData.PlayerDied && t.position.x > 11f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch (faction) {
            case Faction.Wolf:
                OtterCombo = 0;
                EagleCombo = 0;
                WolfV.Flash();
                OtterC.StopCombo();
                EagleC.StopCombo();
                Shards.ExplodeAt(transform.position, Color.red, 3);
                AudioManager.PlaySFX(SFX.GhostBad);
                break;
            case Faction.Otter:
                OtterCombo++;
                EagleCombo = 0;
                OtterV.Flash();
                EagleC.StopCombo();
                Score.AddScore(OtterCombo * OtterCombo * (Movement.Player.currentClip == "DiagAttack" ? 8000 : 1000));
                if (OtterCombo > 3)
                    OtterC.CreateComboText(OtterCombo);
                Shards.ExplodeAt(transform.position, Color.green, 3);
                AudioManager.PlaySFX(SFX.GhostGood);
                break;
            case Faction.Eagle:
                EagleCombo++;
                OtterCombo = 0;
                EagleV.Flash();
                OtterC.StopCombo();
                Score.AddScore(EagleCombo * EagleCombo * (Movement.Player.currentClip == "DiagAttack" ? 8000 : 1000));
                if (EagleCombo > 3)
                    EagleC.CreateComboText(EagleCombo);
                Shards.ExplodeAt(transform.position, new Color(0.4f, 0, 0.5f), 3);
                AudioManager.PlaySFX(SFX.GhostGood);
                break;
        }
        Destroy(gameObject);
    }
}
