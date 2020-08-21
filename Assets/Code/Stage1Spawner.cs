using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Spawner : MonoBehaviour {

    GameObject groundL;
    GameObject groundM;
    GameObject groundR;

    GameObject[] ghosts;
    GameObject[] goodGhosts;

    float[] ghostCoordsTop = { 1.5f, 2.8f, 4.1f };
    float[] ghostCoordsBottom = { -3.66f, -2.36f };
    float[][] ghostHorizontals = { new[] { -0.8f, 0.9f }, new[] { 0f } };

    private void Awake() {
        groundL = Resources.Load<GameObject>("Prefabs/Stage1/groundL");
        groundM = Resources.Load<GameObject>("Prefabs/Stage1/groundM");
        groundR = Resources.Load<GameObject>("Prefabs/Stage1/groundR");
        ghosts = new[] {
            Resources.Load<GameObject>("Prefabs/Wolf"),
            Resources.Load<GameObject>("Prefabs/Otter"),
            Resources.Load<GameObject>("Prefabs/Eagle")
        };
        goodGhosts = new[] { ghosts[1], ghosts[2] };
    }

    public int delay = 0;

    void Update() {
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
            SpawnStructure(4, 0);
            GameData.PlayerDied = false;
        }
    }

    // That probabilty isn't the actual probability, but does affect it monotonically
    // and forbids at 0, good enough.
    /// <summary> Lies and actually spawns something of length length+2. </summary>
    int SpawnStructure(int length, float ghostProbability) {
        Vector3 offSet = new Vector3(11f, 0f, 0f);
        GameObject obj = Instantiate(groundL);
        obj.transform.position += offSet;
        offSet.x += 3.59f;
        // Middle pieces. Here interesting stuff may happen
        for (int i = 0; i < length; i++) {
            obj = Instantiate(groundM);
            obj.transform.position += offSet;

            // Lower ghosts
            if (Random.Range(0f,1f) < ghostProbability) {
                float[] hFormation = ghostHorizontals[Random.Range(0, 2)];
                foreach(float f in ghostCoordsBottom) {
                    if (Random.Range(0f, 1f) < 0.5f) {
                        foreach (float f2 in hFormation) {
                            obj = Instantiate(ghosts.GetRandomEntry());
                            obj.transform.position += offSet + new Vector3(f2, f);
                        }
                    } else {
                        foreach (float f2 in hFormation) {
                            obj = Instantiate(goodGhosts.GetRandomEntry());
                            obj.transform.position += offSet + new Vector3(f2, f);
                        }
                    }
                }
            }

            // Upper ghosts
            if (Random.Range(0f, 1f) < ghostProbability) {
                float[] hFormation = ghostHorizontals[Random.Range(0, 2)];
                foreach (float f in ghostCoordsTop) {
                    if (Random.Range(0f, 1f) < 0.6f) {
                        foreach (float f2 in hFormation) {
                            obj = Instantiate(ghosts.GetRandomEntry());
                            obj.transform.position += offSet + new Vector3(f2, f);
                        }
                    }
                }
            }

            offSet.x += 3.42f;
        }
        // The end pieces may have a few ghosts as well
        offSet.x += 0.16f;
        obj = Instantiate(groundR);
        obj.transform.position += offSet;

        // Lower ghosts
        if (Random.Range(0f, 1f) < ghostProbability) {
            float f2 = ghostHorizontals[0][1];
            obj = Instantiate(ghosts.GetRandomEntry());
            obj.transform.position += offSet + new Vector3(f2, ghostCoordsBottom[0]);
        }

        // Upper ghosts
        if (Random.Range(0f, 1f) < ghostProbability) {
            float[] hFormation = ghostHorizontals[Random.Range(0, 2)];
            foreach (float f in ghostCoordsTop) {
                if (Random.Range(0f, 1f) < 0.4f) {
                    foreach (float f2 in hFormation) {
                        obj = Instantiate(ghosts.GetRandomEntry());
                        obj.transform.position += offSet + new Vector3(f2, f);
                    }
                }
            }
        }

        offSet.x += 3.58f;
        return Mathf.CeilToInt((offSet.x - 11f) / EverythingMoves.MoveSpeed);
    }
}