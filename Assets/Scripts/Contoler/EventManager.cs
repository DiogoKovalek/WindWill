using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    [SerializeField] private CameraControler cameraControler;
    [SerializeField] private UIControler uiControler;
    [SerializeField] private WindControler windControler;
    [SerializeField] private Stopwatch stopwatch;
    [SerializeField] private GameControler gameControler;
    [SerializeField] private EnergyExpenditureControler energyExpenditureControler;
    [SerializeField] private PlayerControler playerControler;
    [SerializeField] private Batery batery;
    [SerializeField] private GameObject objWindTurbine;
    private WindTurbine[] listWindTurbine;
    void Awake() {
        getLists();
        startEvents();
    }
    private void getLists() {
        listWindTurbine = objWindTurbine?.GetComponentsInChildren<WindTurbine>();
    }

    private void startEvents() {
        //CameraControler
        cameraControler.ActivedArrowLeft += uiControler.OnActiveArrowLeft;
        cameraControler.ActivedArrowRight += uiControler.OnActiveArrowRight;
        cameraControler.MovedPlayerByDirection += playerControler.OnMovePlayerByDirection;

        //UIControler
        uiControler.ClickedArrowLeft += cameraControler.OnMoveCameraToLeft;
        uiControler.ClickedArroRight += cameraControler.OnMoveCameraToRight;

        //GameControler
        gameControler.ShowedUILevelComplete += uiControler.OnShowUILevelComplete;
        gameControler.ShowedUIGameOver += uiControler.OnShowUIGameOver;

        //Batery
        batery.GameOvered += gameControler.OnGameOver;

        //WindTurbine
        foreach (var windTurbine in listWindTurbine) {
            windTurbine.energyGenerated += batery.OnReciveEnergy;
            windTurbine.UpdatedTextPercentGerador += uiControler.OnUpdatedTextPercentGerador;
            windTurbine.UpdatedImageColorTurbine += uiControler.OnUpdatedColorImageTurbine;
        }

        //EnergyExpenditureControler
        energyExpenditureControler.ExpecturedEnergy += batery.OnExpecturedEnergy;
        energyExpenditureControler.UpdatedTextSpendHouses += uiControler.OnUpdatedTextSpendHouses;

        //WindControler
        windControler.UpdatedTextSpeedWind += uiControler.OnUpdatedTextSpeedWind;
        windControler.UpdatedImageColorWind += uiControler.OnUpdatedColorImageWind;

        //stopwatch
        stopwatch.TimeOvered += gameControler.OnTimeOver;
        stopwatch.UpdatedTimeUI += uiControler.OnUpdateTime;
    }
}
