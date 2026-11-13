using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerControler : MonoBehaviour {
    [Header("Instancies")]
    [SerializeField] private GameObject objSprite;
    private SpriteRenderer sprRen;
    private Animator anim;

    [Header("Move Points")]
    [SerializeField] private GameObject movePoints;
    private Transform[] listPoints;
    private Coroutine coroutineMovePlayer;

    [Header("Move to Fix")]
    [SerializeField] private float timeToGoInTurbine = 1f;
    [SerializeField] private float timeAlpha = 0.5f;
    private bool isFixingProcess = false;
    private bool isPlayeInAerogerador = false;
    #region EVENTS
    public event Action<bool> DisabledArrowsToFix = delegate { };
    #endregion

    void Awake() {
        if (objSprite != null) {
            anim = objSprite?.GetComponent<Animator>();
            sprRen = objSprite?.GetComponent<SpriteRenderer>();
        }
        if (movePoints != null) {
            listPoints = new Transform[movePoints.transform.childCount];
            for (int i = 0; i < movePoints.transform.childCount; i++) listPoints[i] = movePoints.transform.GetChild(i);
        }
    }

    //Movimentacao esta conectado com a camera
    public void OnMovePlayerByDirection(int ToIndex, float timeToMove) {
        if (coroutineMovePlayer == null) coroutineMovePlayer = StartCoroutine(movePlayer(ToIndex, timeToMove));
        else { // Para o movimento e vai direto para o proximo
            StopCoroutine(coroutineMovePlayer);
            coroutineMovePlayer = StartCoroutine(movePlayer(ToIndex, timeToMove));
        }
    }
    private IEnumerator movePlayer(int ToIndex, float timeToMove) {
        if (isFixingProcess) yield break; // Não permite se mover enquanto estiver consertando
        float t = 0;
        Vector3 startPos = gameObject.transform.position;
        Vector3 targetPos = listPoints[ToIndex].position;

        //Animation
        Vector2 direction = ((Vector2)targetPos - (Vector2)startPos).normalized;
        if (direction.x > 0) sprRen.flipX = false;
        else if (direction.x < 0) sprRen.flipX = true;
        anim.SetBool("IsMove", true);

        while (t < timeToMove) {
            t += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startPos, targetPos, t / timeToMove);
            yield return null;
        }
        gameObject.transform.position = targetPos;
        anim.SetBool("IsMove", false);
        coroutineMovePlayer = null;
    }
    private IEnumerator movePlayerToPoint(Vector3 start, Vector3 target, Action isEnd) {
        //Animation
        Vector2 direction = ((Vector2)target - (Vector2)start).normalized;
        if (direction.x > 0) sprRen.flipX = false;
        else if (direction.x < 0) sprRen.flipX = true;
        anim.SetBool("IsMove", true);

        for(float t = 0; t < timeToGoInTurbine; t += Time.deltaTime) {
            gameObject.transform.position = Vector3.Lerp(start, target, t / timeToGoInTurbine);
            yield return null;
        }
        
        gameObject.transform.position = target;
        anim.SetBool("IsMove", false);

        isEnd();
    }

    #region Conserto de aerogerador

    // === Odem cronologica ===
    // Player recebe o evento
    // Player vai até o Aerogerador
    // Player entra no aerogerador
    // Player conserta o aerogerador (primeiramente com tempo, depois pelos mini game)
    // Player sai do aerogerado
    // Player volta para a posicao inicial
    public void OnPlayerStartToFixTurbine(WindTurbine windTurbine) {
        if (isFixingProcess || !windTurbine.isBroken) return;
        // Inicia corrotina para gerenciar o processo de conserto
        StartCoroutine(PlayerProcessFixTurbine(windTurbine));
    }

    private IEnumerator PlayerProcessFixTurbine(WindTurbine windTurbine) {
        bool aux = false;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = windTurbine.possitionDoor;
        isFixingProcess = true;
        DisabledArrowsToFix?.Invoke(false);

        // Vai até o aerogerador
        aux = false;
        StartCoroutine(movePlayerToPoint(initialPosition, targetPosition, () => {aux = true;}));
        yield return new WaitUntil(() => aux == true);

        // Entra no aerogerador
        isPlayeInAerogerador = true;

        // Inicia o conserto e espera finalizar
        windTurbine.StartFixTurbine(OnPlayerEndToFixTurbine);
        yield return new WaitUntil(() => isPlayeInAerogerador == false);

        // Sai do aerogerador
        StartCoroutine(movePlayerToPoint(targetPosition, initialPosition, () => {aux = true;}));
        yield return new WaitUntil(() => aux == true);
        // Volta para a posicao inicial

        isFixingProcess = false;
        DisabledArrowsToFix?.Invoke(true);
    }

    public void OnPlayerEndToFixTurbine() {
        if (!isPlayeInAerogerador) return;
        isPlayeInAerogerador = false;
    }
    #endregion
}
