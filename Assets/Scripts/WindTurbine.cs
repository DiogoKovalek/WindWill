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
    [Header("Broken")]
    [SerializeField] private bool isFreeForBroken = true;
    [SerializeField] private float minTimeBroken = 5f;
    [SerializeField] private float maxTimeBroken = 15f;
    [SerializeField] private float timeToFix = 3f;
    private bool isBroken = false;
    private bool isFixing = false;
    [SerializeField] private GameObject IconToFix;
    [SerializeField] private GameObject IconFixing;

    #region EVENTS
    public event Action<float> energyGenerated = delegate {};
    public event Action<float, int> UpdatedTextPercentGerador = delegate {};
    public event Action<COLOR_ICON, int> UpdatedImageColorTurbine = delegate {};
    #endregion

    void Start() {
        if (WindControler.windControler != null) {
            StartCoroutine(timerProduceEnergy());
        }
        //Temporario
        UpdatedImageColorTurbine?.Invoke(COLOR_ICON.GREN, indexTurbine);

        //Iniciar cronometro de quebrar
        if(isFreeForBroken) randomizeBrokenTime();
    }

    void Update() {
        if (WindControler.windControler != null) {
            if (WindControler.windControler.windSpeed >= WindControler.windControler.windProduce[5] ||
                WindControler.windControler.windSpeed < WindControler.windControler.windProduce[1]||
                isBroken) isStop = true;
            else {
                isStop = false;
                Paletas?.transform.Rotate(Vector3.forward * WindControler.windControler.windSpeed * speedMultiplier * Time.deltaTime);
            }
        }
    }
    private void randomizeBrokenTime() {
        float timeToBreak = UnityEngine.Random.Range(minTimeBroken, maxTimeBroken);
        StartCoroutine(brokenTimer(timeToBreak));
    }

    private IEnumerator brokenTimer(float time) {
        yield return new WaitForSeconds(time);
        brokenTurbine();
    }
    private void brokenTurbine() {
        isBroken = true;
        isFixing = false;
        UpdatedImageColorTurbine?.Invoke(COLOR_ICON.RED, indexTurbine);
        IconToFix.SetActive(true);
    }

    private void fixTurbine() {
        isBroken = false;
        isFixing = false;
        UpdatedImageColorTurbine?.Invoke(COLOR_ICON.GREN, indexTurbine);
        IconFixing.SetActive(false);
        randomizeBrokenTime();
    }

    public void StartFixTurbine() {
        if(isBroken && !isFixing) {
            isFixing = true;
            IconToFix.SetActive(false);
            IconFixing.SetActive(true);
            StartCoroutine(delayFixTurbine());
        }
    }
    private IEnumerator delayFixTurbine() {
        yield return new WaitForSeconds(timeToFix);
        fixTurbine();
    }
    private IEnumerator timerProduceEnergy() {
        while (true) {
            yield return new WaitForSeconds(1f);
            if (isStop) continue;
            float energy = calculateEnergyBySpeedWind(WindControler.windControler.windSpeed);
            energyGenerated?.Invoke(energy);
            UpdatedTextPercentGerador?.Invoke(energy, indexTurbine);
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
