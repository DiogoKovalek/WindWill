using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIControler : MonoBehaviour {
    //Arrows ===========================================
    [Header("Buttons Arrows")]
    [SerializeField] private GameObject btArrowLeft;
    [SerializeField] private GameObject btArrowRight;
    //==================================================

    //Panel ============================================
    //Textos
    [SerializeField] private TextMeshProUGUI textSpeedWind;
    [SerializeField] private TextMeshProUGUI textSpendHouses;
    [SerializeField] private TextMeshProUGUI textPercentGerador;

    //==================================================

    #region EVENTS
    public event Action ClickedArrowLeft = delegate { };
    public event Action ClickedArroRight = delegate { };
    #endregion
    
    #region Textos
    public void OnUpdatedTextSpeedWind(float speed) {
        textSpeedWind.text = $"{speed:F2} m/s";
    }
    public void OnUpdatedTextSpendHouses(float percent) {
        textSpendHouses.text = $"{percent:F0}%";
    }
    public void OnUpdatedTextPercentGerador(float percent) {
        textPercentGerador.text = $"{percent:0.##}%";
    }
    
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