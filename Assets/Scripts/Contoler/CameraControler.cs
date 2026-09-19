using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {
    [SerializeField] private int screenIndex = 0; // comeca no index 0
    private const int maxScreenIndex = 2;
    private const float distBeteenScrens = 32;
    private const float timeToScroolScreen = 0.5f;
    private Coroutine coroutineMovementCam = null;

    #region EVENTS
    public event Action<bool> ActivedArrowLeft = delegate { };
    public event Action<bool> ActivedArrowRight = delegate { };
    public event Action<int, float> MovedPlayerByDirection = delegate { };
    #endregion

    void Start() {
        checkIfIsFreeLeftAndRight();
    }

    public void OnMoveCameraToLeft() {
        if (screenIndex <= 0) return; // não pode se mover para a esquerda se nao tem tela
        if (coroutineMovementCam == null) coroutineMovementCam = StartCoroutine(movementCam(-1));
        MovedPlayerByDirection?.Invoke(screenIndex - 1, timeToScroolScreen);
    }
    public void OnMoveCameraToRight() {
        if (screenIndex >= maxScreenIndex) return; // não pode se mover para a direita se nao tem tela
        if (coroutineMovementCam == null) coroutineMovementCam = StartCoroutine(movementCam(1));
        MovedPlayerByDirection?.Invoke(screenIndex + 1, timeToScroolScreen);
    }

    public IEnumerator movementCam(int dir) {
        float t = 0;
        Vector3 startPos = gameObject.transform.position;
        Vector3 targetPos = startPos + new Vector3(distBeteenScrens * dir, 0, 0);
        while (t < timeToScroolScreen) {
            t += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startPos, targetPos, t / timeToScroolScreen);
            yield return null;
        }
        gameObject.transform.position = targetPos;
        coroutineMovementCam = null;
        screenIndex += dir;
        checkIfIsFreeLeftAndRight();
    }

    private void checkIfIsFreeLeftAndRight() { // verificar se exite tela para direita e para a esquerda
        if (screenIndex <= 0) ActivedArrowLeft(false);
        else ActivedArrowLeft(true);
        if (screenIndex >= maxScreenIndex) ActivedArrowRight(false);
        else ActivedArrowRight(true);
    }
}

