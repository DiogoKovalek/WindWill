using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private Color32[] colorsIcons;
    [SerializeField] private Image[] imagesIconsTurbine;
    [SerializeField] private Image imageIconWind;
    // Soma das turbinas
    private float[] turbinesProduces = new float[4];

    //==================================================

    #region EVENTS
    public event Action ClickedArrowLeft = delegate { };
    public event Action ClickedArroRight = delegate { };
    #endregion
    
    #region Textos
    public void OnUpdatedTextSpeedWind(float speed) {
        textSpeedWind.text = $"{speed:F2} m/s";
    }
    public void OnUpdatedColorImageWind(COLOR_ICON colorIcon) {
        switch (colorIcon) {
            case COLOR_ICON.GREN:
                imageIconWind.color = colorsIcons[0];
                break;
            case COLOR_ICON.YELLOW:
                imageIconWind.color = colorsIcons[1];
                break;
            case COLOR_ICON.RED:
                imageIconWind.color = colorsIcons[2];
                break;
            default:
                imageIconWind.color = colorsIcons[0];
                break;
        }
    }
    public void OnUpdatedTextSpendHouses(float percent) {
        textSpendHouses.text = $"{percent:F0}%";
    }
    public void OnUpdatedTextPercentGerador(float percent, int indexTurbine) {
        turbinesProduces[indexTurbine] = percent;
        textPercentGerador.text = $"{turbinesProduces.Sum():0.##}%";
    }
    public void OnUpdatedColorImageTurbine(COLOR_ICON colorIcon, int indexTurbine) {
        Image imageIconTurbine = imagesIconsTurbine[indexTurbine];
        switch (colorIcon) {
            case COLOR_ICON.GREN:
                imageIconTurbine.color = colorsIcons[0];
                break;
            case COLOR_ICON.YELLOW:
                imageIconTurbine.color = colorsIcons[1];
                break;
            case COLOR_ICON.RED:
                imageIconTurbine.color = colorsIcons[2];
                break;
            default:
                imageIconTurbine.color = colorsIcons[0];
                break;
        }
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

public enum COLOR_ICON {
    GREN,
    YELLOW,
    RED
}