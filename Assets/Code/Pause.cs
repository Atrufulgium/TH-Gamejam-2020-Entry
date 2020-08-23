using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    static Pause p;
    public GameObject DiedDark;
    public GameObject TitleDark;

    private void Awake() {
        p = this;
    }

    void Update() {
        if (MenuItem.AllowInteraction && Input.GetKeyDown(KeyCode.Escape)) {
            TitleDark.SetActive(true);
            GameData.Paused = true;
            //GameData.Paused = !GameData.Paused;
            Time.timeScale = GameData.Paused ? 0 : 1;
            StartCoroutine(toTitle());
        }
    }

    public static void Die() {
        p.StartCoroutine(p.die());
    }

    IEnumerator die() {
        for (int i = 0; i < 60; i++)
            yield return null;
        DiedDark.SetActive(true);
        for (int i = 0; i < 120; i++)
            yield return null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator toTitle() {
        for (int i = 0; i < 30; i++)
            yield return null;
        GameData.Paused = false;
        MenuItem.AllowInteraction = true;
        Time.timeScale = 1;
        AudioManager.PlayMusic(Music.Title);
        SceneManager.LoadScene(0);
    }
}
