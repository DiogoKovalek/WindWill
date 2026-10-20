using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuControler : MonoBehaviour {
    public void Start() {
        if (InputManager.inputManager != null) {
            InputManager.inputManager.tradeMap(ACTION_MAP.MENU);
        }
    }
    public void OnEnable() {
        if(InputManager.inputManager != null) {
            InputManager.inputManager.StartedGame += OnStartGame;
        }
    }
    public void OnDisable() {
        if(InputManager.inputManager != null) {
            InputManager.inputManager.StartedGame -= OnStartGame;
        }
    }
    public void OnStartGame() {
        LevelManager.MinigameLevel();
    }
}
