using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int Stage;
    public Text Highscore;
    RectTransform HighscoreRecttr;
    public static long PlayerScore = 0;
    static Score s;

    long visibleScore = 0;
    long storedHiscore = 0;
    long initHiscore = 0;
    bool scoreDirty = false;
    string lang;
    string hilang;

    RectTransform rt;
    Text t;

    public bool beatHighscore = false;

    public static string ScoreString(long amount) {
        return amount.ToString("### ### ### ### ### ### ### ##0").Trim();
    }

    void Start() {
        s = this;
        rt = GetComponent<RectTransform>();
        t = GetComponent<Text>();
        HighscoreRecttr = Highscore.GetComponent<RectTransform>();
        lang = Localiser.GetString("score");
        hilang = Localiser.GetString("hiscore");
        scoreDirty = true;
        storedHiscore = GetHighscore();
        initHiscore = storedHiscore;
        Highscore.text = $"<size=40>{hilang}</size> {ScoreString(storedHiscore)}";
    }

    private void LateUpdate() {
        if (scoreDirty) {
            t.text = $"<size=60>{lang}</size> {ScoreString(visibleScore)}";
            if (beatHighscore) {
                if (initHiscore > visibleScore)
                    Highscore.text = $"<size=40>{hilang}</size> {ScoreString(initHiscore)}";
                else
                    Highscore.text = $"<size=40>{hilang}</size> {ScoreString(visibleScore)}";
            }
            scoreDirty = false;
        }
    }

    public static void AddScore(long amount) {
        s.addScore(amount);
    }

    void addScore(long amount) {
        // Make sure it's a nice round number.
        amount = (amount / 50) * 50;
        if (!beatHighscore && PlayerScore + amount > GetHighscore()) {
            beatHighscore = true;
        }
        PlayerScore += amount;
        if (beatHighscore)
            SetHighscore(PlayerScore);
        StartCoroutine(AddScoreOverTime(amount));
    }

    IEnumerator AddScoreOverTime(long amount) {
        long delta = amount / 50;
        for (int i = 0; i < 50; i++) {
            visibleScore += delta;
            scoreDirty = true;
            yield return null;
        }
    }

    IEnumerator GrowShrink() {
        while (true) {
            if (scoreDirty) {
                bool beat = beatHighscore;
                for (int i = 0; i < 50; i += 2) {
                    Vector3 scale = Vector3.one * (1 + (1 - Mathf.Cos(i / 25f * 2 * Mathf.PI)) / 20f);
                    rt.localScale = scale;
                    if (beat)
                        HighscoreRecttr.localScale = scale;
                    yield return null;
                    yield return null;
                }
            } else {
                yield return null;
            }
        }
    }

    long GetHighscore() {
        switch (Stage) {
            case 1: return GameSaveable.Stage1HiScore;
            case 3: return GameSaveable.Stage3HiScore;
        }
        return 0;
    }

    void SetHighscore(long score) {
        switch (Stage) {
            case 1: GameSaveable.Stage1HiScore = score; break;
            case 3: GameSaveable.Stage3HiScore = score; break;
        }
    }
}
