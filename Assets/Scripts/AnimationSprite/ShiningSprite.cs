using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiningSprite : MonoBehaviour {
    [SerializeField] private GameObject objSprite;
    [SerializeField] private float timeToShining = 1;
    private float t = 0;
    void Update() {
        t += Time.deltaTime;
        if(t >= timeToShining) {
            objSprite.SetActive(!objSprite.activeSelf);
            t = 0;
        }
    }
}
