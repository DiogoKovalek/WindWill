using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private CameraControler cameraControler;
    [SerializeField] private UIControler uiControler;
    [SerializeField] private PlayerControler playerControler;
    void Awake()
    {
        startEvents();
    }

    private void startEvents() {
        //CameraControler
        cameraControler.ActivedArrowLeft += uiControler.OnActiveArrowLeft;
        cameraControler.ActivedArrowRight += uiControler.OnActiveArrowRight;
        cameraControler.MovedPlayerByDirection += playerControler.OnMovePlayerByDirection;

        //UIControler
        uiControler.ClickedArrowLeft += cameraControler.OnMoveCameraToLeft;
        uiControler.ClickedArroRight += cameraControler.OnMoveCameraToRight;
    }
}
