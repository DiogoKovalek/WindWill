using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour {
    //===================================================
    //Controler vai servir tambem como entrada do jogador
    //===================================================

    private bool endGame = false;

    #region EVENTS
    public event Action ShowedUILevelComplete = delegate { };
    public event Action ShowedUIGameOver = delegate { };
    #endregion
    void Start() {
        if (InputManager.inputManager != null) {
            InputManager.inputManager.tradeMap(ACTION_MAP.GAME);
        }
    }
    void OnEnable() {
        if (InputManager.inputManager != null) {
            InputManager.inputManager.ClickedInSomething += OnClickInSomething;
            InputManager.inputManager.ExitedGame += OnExitGame;
        }
    }
    void OnDisable() {
        if (InputManager.inputManager != null) {
            InputManager.inputManager.ClickedInSomething -= OnClickInSomething;
            InputManager.inputManager.ExitedGame -= OnExitGame;
        }
    }
    public void OnTimeOver() {
        if (!endGame) {
            levelComplete();
        }
    }
    public void OnGameOver() {
        if (!endGame) {
            gameOver();
        }
    }
    private void levelComplete() {
        endGame = true;
        ShowedUILevelComplete?.Invoke();
    }
    private void gameOver() {
        endGame = true;
        ShowedUIGameOver?.Invoke();
    }
    public void OnExitGame() {
        LevelManager.ToGoHomeScreen();
    }
    public void OnClickInSomething(RaycastHit2D hit) {
        Debug.Log("Hit");
    }
}
