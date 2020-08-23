using UnityEngine;
using UnityEngine.UI;

public class MenuVolChanger : MonoBehaviour {

    // otherwise sfx
    public bool inited = false;
    public bool isBgm;
    Text t;

    void Start() {
        t = GetComponent<Text>();
    }

    public int time = 60;
    
    void Update() {
        if (!inited) {
            inited = true;
            UpdateText();
        }
        time++;
        if (!isBgm && time >= 60) {
            AudioManager.PlaySFX(SFX.Jump);
            time = 0;
        }
        if (Input.GetKey(GameData.Right)) {
            if (isBgm)
                AudioManager.SetBGMVolume(GameSaveable.bgmVol + 0.033f);
            else
                AudioManager.SetSFXVolume(GameSaveable.sfxVol + 0.033f);
            UpdateText();
        }
        if (Input.GetKey(GameData.Left)) {
            if (isBgm)
                AudioManager.SetBGMVolume(GameSaveable.bgmVol - 0.033f);
            else
                AudioManager.SetSFXVolume(GameSaveable.sfxVol - 0.033f);
            UpdateText();
        }
    }

    void UpdateText() {
        float val = isBgm ? GameSaveable.bgmVol : GameSaveable.sfxVol;
        t.text = ((int)(val * 100)).ToString() + "%";
    }
}
