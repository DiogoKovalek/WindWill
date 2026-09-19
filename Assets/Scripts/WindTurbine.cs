using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbine : MonoBehaviour {
    [SerializeField] private GameObject Paletas;
    private const float speedMultiplier = 1.5f;
    private bool isStop = false;

    [Header("Energy")]
    private float energyMultiplier = 0.005f;

    #region EVENTS
    public event Action<float> energyGenerated;
    #endregion

    void Start() {
        if (WindControler.windControler != null) {
            StartCoroutine(timerProduceEnergy());
        }
    }

    void Update() {
        if (WindControler.windControler != null) {
            if (WindControler.windControler.windSpeed >= WindControler.windControler.windProduce[4] || 
                WindControler.windControler.windSpeed < WindControler.windControler.windProduce[1]) isStop = true;
            else {
                isStop = false;
                Paletas?.transform.Rotate(Vector3.forward * WindControler.windControler.windSpeed * speedMultiplier * Time.deltaTime);
            }
        }
    }

    private IEnumerator timerProduceEnergy() {
        while (true) {
            yield return new WaitForSeconds(1f);
            if (isStop) continue;
            float energy = calculateEnergyBySpeedWind(WindControler.windControler.windSpeed);
            energyGenerated?.Invoke(energy);
        }
    }

    private float calculateEnergyBySpeedWind(float windSpeed) {
        // 0 -> 1 Nao produz
        // 1 -> 2 Produz com uma porcentagem abaixo de 100%
        // 2 -> 4 Produz com 100%
        // 4 -> Infinity Não produz cut-out
        float[] p = WindControler.windControler.windProduce;
        float percent = 0;

        // Abaixo da velocidade minima e acima da maxima
        if (windSpeed < p[1] || windSpeed >= p[4]) {
            return 0;
        }
        // Primeiro intervalo de producao -> abaixo de 100%
        if (windSpeed < p[2]) {
            percent = Mathf.InverseLerp(p[1], p[2], windSpeed) * 100;
        } 
        // Segundo intervalo de producao -> exatamente 100%
        else if (windSpeed < p[4]) {
            percent = 100;
        }
        return percent * energyMultiplier;

    }
}
