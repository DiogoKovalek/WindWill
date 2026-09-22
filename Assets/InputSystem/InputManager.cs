using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour{

    public static InputManager inputManager;
    private PlayerInput playerInput;

    #region EVENTS
    #region Menu Events
    public event Action StartedGame = delegate {};
    #endregion
    #region Game Events
    #endregion
    #region MiniGame Events
    #endregion
    #endregion

    void Awake() {
        if(inputManager == null) {
            inputManager = this;
            DontDestroyOnLoad(this.gameObject);

            playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = true;
        }
        else {
            Destroy(this.gameObject);
            return;
        }
    }

    void OnEnable() {
        
    }
    void OnDisable() {
        
    }

    #region Global
    public void tradeMap(ACTION_MAP actionMap) {
        String act = "";
        switch (actionMap) {
            case ACTION_MAP.MENU:
                act = "MenuInicial";
                break;
            case ACTION_MAP.GAME:
                act = "Game";
                break;
            case ACTION_MAP.MINIGAME:
                act = "MiniGame";
                break;
            default:
                return;
        }
        playerInput.SwitchCurrentActionMap(act);
    }

    public bool isClickInButtonUI() {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }
        Debug.Log("Clicou em Botao");
        return false;
    }
    #endregion

    #region Menu
    public void OnStartGame() {
        Debug.Log("C");
        if (isClickInButtonUI()) return;
        Debug.Log("Clicou");
        StartedGame?.Invoke();
    }
    #endregion

    #region Game
    #endregion

    #region MiniGame
    #endregion
}

public enum ACTION_MAP {
    MENU,
    GAME,
    MINIGAME
}
