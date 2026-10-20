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
    public float windSpeed = 8;
    private byte indexForce = 2; // valor de 1 a 3 para representar Vento Fraco/Medio/Forte

    #region EVENTS
    public event Action<float> UpdatedTextSpeedWind = delegate { };
    public event Action<COLOR_ICON> UpdatedImageColorWind = delegate { };
    #endregion

    // Wind Varibles ===============================================
    public float[] windProduce = { 3, 7, 10, 14 };

    [Header("Times")]
    [SerializeField] private float minTimeToTradeIndexForce;
    [SerializeField] private float maxTimeToTradeIndexForce;
    [SerializeField] private float timeDelayUpdateSpeed;

    [Header("Targets")]
    [SerializeField] private float VariationSpeed = 0.5f;
    [SerializeField] private float VariationSpeedOutRange = 1;
    private byte toIndexForce;
    //==============================================================
    void Awake() {
        if (windControler == null) windControler = this;
        else Destroy(this);

        
        StartCoroutine(corrotineUpdateSpeedWind());
        StartCoroutine(corrotineTradeIndexForce());
    }
    void Start() {
        UpdatedTextSpeedWind?.Invoke(windSpeed);
    }

    IEnumerator corrotineUpdateSpeedWind() {
        if (windSpeed < windProduce[indexForce - 1] || windSpeed >= windProduce[indexForce]) { // O vento não está no intervalo do indexForce
            // Verificar se o intervalo está maior ou menor
            // Adicionar + 1 para tentar chegar ao intervalo
            windSpeed += windSpeed < windProduce[indexForce - 1] ? VariationSpeedOutRange : -VariationSpeedOutRange;

            //Se chegou ao intervalo, execute a corrotina de troca de indexForce novamente
            if (windSpeed >= windProduce[indexForce - 1] && windSpeed < windProduce[indexForce]) {
                StartCoroutine(corrotineTradeIndexForce());
            }
        }
        else { // O vento está no intervalo do indexForce;
            byte signal = (byte)UnityEngine.Random.Range(0, 2);
            float speedVariation = VariationSpeed;
            if (signal == 1) {
                speedVariation *= -1;
            }

            //Verificar se a variacao está no limite
            float newWindSpeed = windSpeed + speedVariation;
            if (newWindSpeed >= windProduce[indexForce - 1] && newWindSpeed < windProduce[indexForce]) {
                windSpeed = newWindSpeed;
            }
            else {
                speedVariation *= -1;
                newWindSpeed = windSpeed + speedVariation; // A ideia é que max variation nunca seja maior que a metade do tamanho do intervalo, no caso 1.5
                windSpeed = newWindSpeed;
            }
        }

        // Atualizar UI
        UpdatedTextSpeedWind?.Invoke(windSpeed);
        if (windSpeed < windProduce[1] || windSpeed > windProduce[3]) {
            UpdatedImageColorWind?.Invoke(COLOR_ICON.RED);
        }
        else if (windSpeed < windProduce[2]) {
            UpdatedImageColorWind?.Invoke(COLOR_ICON.YELLOW);
        }
        else if (windSpeed <= windProduce[3]) {
            UpdatedImageColorWind?.Invoke(COLOR_ICON.GREN);
        }

        yield return new WaitForSeconds(timeDelayUpdateSpeed);
        StartCoroutine(corrotineUpdateSpeedWind());
    }

    IEnumerator corrotineTradeIndexForce() {
        float timeToTradeIndexFoce = UnityEngine.Random.Range(minTimeToTradeIndexForce, maxTimeToTradeIndexForce);
        yield return new WaitForSeconds(timeToTradeIndexFoce);

        Debug.Log("Troca o intervalo");
        if (indexForce == 1) {
            indexForce++;
        }
        else if (indexForce == 3) {
            indexForce--;
        }
        else {
            byte direction = (byte)UnityEngine.Random.Range(0, 2); // 0 -> decress 1 -> acress
            indexForce += (byte)(direction == 0 ? -1 : 1);
        }
    }

}
