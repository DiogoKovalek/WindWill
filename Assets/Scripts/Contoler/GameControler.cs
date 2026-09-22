using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour {
    //===================================================
    //Controler vai servir tambem como entrada do jogador
    //===================================================
    void Start() {
        if(InputManager.inputManager != null) {
            InputManager.inputManager.tradeMap(ACTION_MAP.GAME);
        }
    }
    void OnEnable() {
        if(InputManager.inputManager != null) {
            InputManager.inputManager.ClickedInSomething += OnClickInSomething;
            InputManager.inputManager.ExitedGame += OnExitGame;
        }
    }
    void OnDisable() {
        if(InputManager.inputManager != null) {
            InputManager.inputManager.ClickedInSomething -= OnClickInSomething;
            InputManager.inputManager.ExitedGame -= OnExitGame;
        }
    }
    public void OnGameOver() {
        Debug.Log("Game Over");
    }
    public void OnExitGame() {
        LevelManager.ToGoHomeScreen();
    }
    public void OnClickInSomething(RaycastHit2D hit) {
        Debug.Log("Hit");
    }
}
