using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControler : MonoBehaviour {
    //Arrows ===========================================
    [Header("Buttons Arrows")]
    [SerializeField] private GameObject btArrowLeft;
    [SerializeField] private GameObject btArrowRight;
    //==================================================

    #region EVENTS
    public event Action ClickedArrowLeft = delegate { };
    public event Action ClickedArroRight = delegate { };
    #endregion

    #region Arrows
    public void OnButtonArrowLeft() {
        ClickedArrowLeft?.Invoke();
        OnActiveArrowLeft(false);
        OnActiveArrowRight(false);
    }
    public void OnButtonArrowRight() {
        ClickedArroRight?.Invoke();
        OnActiveArrowLeft(false);
        OnActiveArrowRight(false);
    }
    public void OnActiveArrowLeft(bool isActive) {
        btArrowLeft.SetActive(isActive);
    }
    public void OnActiveArrowRight(bool isActive) {
        btArrowRight.SetActive(isActive);
    }
    #endregion 
}