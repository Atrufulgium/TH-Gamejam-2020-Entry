using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {

    public int Stage;

    private void Start() {
        GetComponent<Text>().text = Score.ScoreString(GetHighscore());
    }


    long GetHighscore() {
        switch (Stage) {
            case 1: return GameSaveable.Stage1HiScore;
            case 3: return GameSaveable.Stage3HiScore;
        }
        return 0;
    }
}