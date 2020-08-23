using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Music { None = -1, Title = 0, Stage1 = 1, Stage3 = 2 }
public enum SFX { Jump = 0, Die = 1, GhostGood = 2, GhostBad = 3, ComboBreak = 4, Step = 5, Confirm = 6, Cancel = 7 }

public class AudioManager : MonoBehaviour {

    public static AudioManager manager;
    // Make sure to align these with the enum.
    public AudioSource[] bgm;
    public AudioSource[] sfx;

    public Music nowPlaying = Music.None;

    void Start() {
        if (manager != null)
            Destroy(gameObject);
        else {
            manager = this;
            DontDestroyOnLoad(gameObject);

            SetBGMVolume(GameSaveable.bgmVol);
            SetSFXVolume(GameSaveable.sfxVol);
            PlayMusic();
        }
    }

    // Plays the correct music.
    public static void PlayMusic() {
        string level = SceneManager.GetActiveScene().name;
        switch (level) {
            case "Menu": PlayMusic(Music.Title); break;
            case "Stage1": PlayMusic(Music.Stage1); break;
            case "Stage3": PlayMusic(Music.Stage3); break;
            default: PlayMusic(Music.None); break;
        }
    }


    // Call with "None" to stop all music.
    public static void PlayMusic(Music m) {
        if (m == manager.nowPlaying)
            return;
        if (manager.nowPlaying != Music.None)
            manager.bgm[(int)manager.nowPlaying].Stop();
        if (m != Music.None)
            manager.bgm[(int)m].Play();
        manager.nowPlaying = m;
    }

    public static void PlaySFX(SFX s) {
        manager.sfx[(int)s].Play();
    }

    public static void SetBGMVolume(float amount) {
        amount = Mathf.Clamp01(amount);
        foreach (AudioSource s in manager.bgm)
            s.volume = amount;
        GameSaveable.bgmVol = amount;
    }

    public static void SetSFXVolume(float amount) {
        amount = Mathf.Clamp01(amount);
        foreach (AudioSource s in manager.sfx)
            s.volume = amount;
        GameSaveable.sfxVol = amount;
    }
}
