using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private CameraControler cameraControler;
    [SerializeField] private UIControler uiControler;
    [SerializeField] private GameControler gameControler;
    [SerializeField] private PlayerControler playerControler;
    [SerializeField] private Batery batery;
    [SerializeField] private GameObject objWindTurbine;
    private WindTurbine[] listWindTurbine;
    void Awake()
    {
        getLists();
        startEvents();
    }
    private void getLists(){
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

        //Batery
        batery.GameOvered += gameControler.OnGameOver;

        //WindTurbine
        foreach(var windTurbine in listWindTurbine) {
            windTurbine.energyGenerated += batery.OnReciveEnergy;
        }

    }
}
