using UnityEngine;

public static class ExtensionMethods {

    public static T GetRandomEntry<T>(this T[] arr) {
        return arr[Random.Range(0, arr.Length)];
    }

}