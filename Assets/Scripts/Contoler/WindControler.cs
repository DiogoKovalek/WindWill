using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindControler : MonoBehaviour {
    //0 a 7 m/a não produz energia
    //4 a 7 m/a apenas gira
    //7 a 10 m/a é a faixa que produz
    //10 a 14 m/a produz com 100% de eficacia
    //acima de 25 m/a é cut-out
    public static WindControler windControler;
    public float windSpeed = 7;

    #region EVENTS
    public event Action<float> UpdatedTextSpeedWind = delegate {};
    #endregion

    // Wind Varibles ===============================================
    public readonly float[] windProduce = { 0f, 4f, 7f, 10, 14, 25};
    //==============================================================
    void Awake() {
        if (windControler == null) windControler = this;
        else Destroy(this);
    }

    //Temporario até mecher no sistema de vento
    void FixedUpdate() {
        UpdatedTextSpeedWind?.Invoke(windSpeed);
    }
}
