using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    
    public static GameManager Instance { get; private set; }

    public float RoadSpeed=5;

    private void Awake() {

        if (Instance == null) {
            Instance = this;
        }
    }
}
