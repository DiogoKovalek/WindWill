using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbine : MonoBehaviour {
    // Ver outra forma de declarar elas
    [SerializeField] private int indexTurbine;
    [SerializeField] private GameObject Paletas;
    private const float speedMultiplier = 2.5f;
    private bool isStop = false;

    [Header("Energy")]
    private float energyPercentProduce = 0.05f;

    #region EVENTS
    public event Action<float, int> energyGenerated = delegate {};
    #endregion

    void Start() {
        if (WindControler.windControler != null) {
            StartCoroutine(timerProduceEnergy());
        }
    }

    void Update() {
        if (WindControler.windControler != null) {
            if (WindControler.windControler.windSpeed >= WindControler.windControler.windProduce[5] ||
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
            energyGenerated?.Invoke(energy, indexTurbine);
        }
    }

    private float calculateEnergyBySpeedWind(float windSpeed) {
        float[] p = WindControler.windControler.windProduce;
        float percent = 0;

        // Abaixo da velocidade minima e acima da maxima
        if (windSpeed < p[2] || windSpeed >= p[5]) {
            return 0;
        }
        // Primeiro intervalo de producao -> abaixo de 100%
        if (windSpeed < p[3]) {
            percent = Mathf.InverseLerp(p[2], p[3], windSpeed) * 100;
        }
        // Segundo intervalo de producao -> exatamente 100%
        else if (windSpeed < p[5]) {
            percent = 100;
        }
        return percent * energyPercentProduce;

    }
}
