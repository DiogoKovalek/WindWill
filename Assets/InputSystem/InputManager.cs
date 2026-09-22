using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    public static InputManager inputManager;
    private PlayerInput playerInput;

    #region EVENTS
    #region Menu Events
    public event Action StartedGame = delegate { };
    #endregion
    
    #region Game Events
    private InputAction mousePositionAction;
    public event Action<RaycastHit2D> ClickedInSomething = delegate {};
    public event Action ExitedGame = delegate {};
    #endregion
    
    #region MiniGame Events
    private byte moveDirection = 0;
    public event Action<byte> MovedDirection = delegate {};
    public event Action SelectedAnswer = delegate {};
    #endregion
    #endregion

    void Awake() {
        if (inputManager == null) {
            inputManager = this;
            DontDestroyOnLoad(this.gameObject);

            playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = true;
            //Pode ser feito outra coisa
            mousePositionAction = playerInput.actions["MousePosition"];
        }
        else {
            Destroy(this.gameObject);
            return;
        }
    }

    void OnEnable() {

    }
    void OnDisable() {
        StartedGame = null;
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
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }
        return false;
    }
    #endregion

    #region Menu
    public void OnStartGame() {
        if (isClickInButtonUI()) return;
        StartedGame?.Invoke();
    }
    #endregion

    #region Game
    public void OnClick() {
        if(isClickInButtonUI()) return;
        // Verificar colisor
        Vector2 mousePosition = mousePositionAction.ReadValue<Vector2>();
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if(hit.collider != null) ClickedInSomething?.Invoke(hit);
    }

    public void OnExitGame() {
        ExitedGame?.Invoke();
    }
    #endregion

    #region MiniGame
    public void OnNavigation(InputValue value) {
        Vector2 vetor = value.Get<Vector2>().normalized;
        if (vetor == Vector2.zero) {
            moveDirection = 0;
            return;
        }
        moveDirection = transformVector2InMoveDirection(vetor);

        MovedDirection?.Invoke(moveDirection);
    }
    public void OnSelectAnswer() {
        SelectedAnswer?.Invoke();
    }
    private byte transformVector2InMoveDirection(Vector2 vectorNormalized) {
        if (vectorNormalized.sqrMagnitude < 0.98) {
            return 0;
        }
        byte move;
        if (Mathf.Abs(vectorNormalized.x) > Mathf.Abs(vectorNormalized.y)) {
            move = vectorNormalized.x > 0 ? (byte)2 : (byte)4;
        }
        else {
            move = vectorNormalized.y > 0 ? (byte)1 : (byte)3;
        }
        return move;
    }
    #endregion
}

public enum ACTION_MAP {
    MENU,
    GAME,
    MINIGAME
}
