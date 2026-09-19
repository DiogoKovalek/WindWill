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

    #region EVENTS

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
}
