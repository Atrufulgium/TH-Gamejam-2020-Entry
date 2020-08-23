using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Spawner : MonoBehaviour {

    GameObject box;

    GameObject[] ghosts;
    GameObject[] goodGhosts;

    float[] ghostCoordsTop = { -0.7f, 0.6f };
    float[][] ghostHorizontals = { new[] { -0.8f, 0.9f }, new[] { 0f } };

    private void Awake() {
        box = Resources.Load<GameObject>("Prefabs/Stage3/Box");
        ghosts = new[] {
            Resources.Load<GameObject>("Prefabs/Wolf"),
            Resources.Load<GameObject>("Prefabs/Otter"),
            Resources.Load<GameObject>("Prefabs/Eagle")
        };
        goodGhosts = new[] { ghosts[1], ghosts[2] };
    }

    public int delay = 0;

    void Update() {
        if (GameData.Paused)
            return;
        delay--;
        if (delay <= 0) {
            // spawn a structure and have at least enough delay for that one to end
            delay = SpawnStructure(Random.Range(0, 6), 0.75f);
            // also add a random gap
            delay += Random.Range(30, 80);
        }
    }

    private void LateUpdate() {
        if (GameData.PlayerDied) {
            delay = Mathf.RoundToInt(19.2f / EverythingMoves.MoveSpeed);
            GameData.PlayerDied = false;
        }
    }

    static int[] heights = { 0, 0, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3};

    // That probabilty isn't the actual probability, but does affect it monotonically
    // and forbids at 0, good enough.
    int SpawnStructure(int length, float ghostProbability) {
        Vector3 offSet = new Vector3(11f, 0f, 0f);
        GameObject obj;
        // Place boxes for length items.
        int prevheight = 0;
        for (int i = 0; i < length; i++, offSet.x += 2) {
            int height = heights.GetRandomEntry();
            if (height - prevheight > 0) {
                height = 2;
            }
            for (int ii = 0; ii < height; ii++) {
                obj = Instantiate(box);
                obj.transform.position += offSet + 2 * ii * Vector3.up;
            }

            // Lower ghosts
            if (Random.Range(0f, 1f) < ghostProbability) {
                float[] hFormation = ghostHorizontals[Random.Range(0, 2)];
                for (int f = height; f < 4; f++) {
                    foreach(float f3 in ghostCoordsTop) {
                        if (Random.Range(0f, 1f) < 0.5f) {
                            foreach (float f2 in hFormation) {
                                obj = Instantiate(ghosts.GetRandomEntry());
                                obj.transform.position += offSet + new Vector3(f2, f*2 + f3);
                            }
                        } else {
                            foreach (float f2 in hFormation) {
                                obj = Instantiate(goodGhosts.GetRandomEntry());
                                obj.transform.position += offSet + new Vector3(f2, f*2 + f3);
                            }
                        }
                    }
                }
            }
        }

        offSet.x += 2f;
        return Mathf.CeilToInt((offSet.x - 11f) / EverythingMoves.MoveSpeed);
    }
}