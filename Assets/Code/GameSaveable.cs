using System;
using System.IO;
using UnityEngine;

[Serializable]
public enum Lang { EN, JA }

[Serializable]
public static class GameSaveable {
    
    public static float bgmVol = 0.5f;
    public static float sfxVol = 0.5f;
    public static Lang language = Lang.JA;

    public static long Stage1HiScore = 0;
    public static long Stage3HiScore = 0;

    public static void SaveData() {
        throw new NotImplementedException();
        //(new FileInfo(GetFolder())).Directory.Create();
        //using (Stream stream = File.Open(GetPath(Name), FileMode.Create)) {
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    formatter.Serialize(stream, this);
        //}
    }
}