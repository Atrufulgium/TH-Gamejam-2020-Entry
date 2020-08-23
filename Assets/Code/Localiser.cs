using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Localiser : MonoBehaviour {

    static Dictionary<string, string> content;
    static bool inited = false;

    static void Init() {
        content = new Dictionary<string, string>();
        string troep = Resources.Load<TextAsset>("lang").text;
        troep.Replace("\r\n", "\n");
        troep.Replace('\r', '\n');
        foreach(string s in troep.Split('\n')) {
            string[] stuff = s.Split(new[] { '=' }, 2, System.StringSplitOptions.None);
            content.Add(stuff[0], stuff[1].Replace("\\n", "\n"));
        }
    }

    private void Awake() {
        if (TryGetComponent<Text>(out Text t)) {
            t.text = GetString(t.text);
        }
    }

    public static string GetString(string key) {
        if (!inited)
            Init();

        Lang lang = GameSaveable.language;
        if (content.TryGetValue($"{key}.{lang.ToString()}", out string val)) {
            return val;
        }
        return key;
    }

}
