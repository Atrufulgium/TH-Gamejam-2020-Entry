using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLangSwap : MonoBehaviour {

    // otherwise sfx
    Text t;

    void Start() {
        t = GetComponent<Text>();
    }

    public int time = 60;

    void Update() {
        if (Input.GetKey(GameData.Right)) {
            switch (GameSaveable.language) {
                case Lang.EN:
                    GameSaveable.language = Lang.JA;
                    break;
                case Lang.JA:
                    GameSaveable.language = Lang.EN;
                    break;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
