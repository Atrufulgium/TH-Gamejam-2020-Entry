using UnityEngine;

public static class GameData {

    public static KeyCode Left = KeyCode.LeftArrow; // Simple left movement
    public static KeyCode Right = KeyCode.RightArrow; // Simple right movement
    public static KeyCode Jump = KeyCode.UpArrow; // Jumping, exiting slides
    public static KeyCode Down = KeyCode.DownArrow; // Diagonal attacks, entering slides

    public static float FieldWidth = 19.2f;
    public static float FieldHeight = 10.8f;
    public static float FieldL = -9.6f;
    public static float FieldR = 9.6f;
    public static float FieldT = 5.4f;
    public static float FieldB = -5.4f;
    public static Bounds FieldBounds = new Bounds(new Vector3(0, 0), new Vector3(FieldWidth, FieldHeight));

    public static float DefaultSpeed = 0.1666667f;

    public static bool PlayerDied = false;
}