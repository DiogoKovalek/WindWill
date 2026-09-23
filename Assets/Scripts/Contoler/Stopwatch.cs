using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopwatch : MonoBehaviour {
    private int timeToSurvive = 30;
    private int time = 0;

    #region EVENTS
    public event Action TimeOvered = delegate { };
    public event Action<int> UpdatedTimeUI = delegate { };
    #endregion
    void Start() {
        time = timeToSurvive;
        StartCoroutine(stopwatch());
    }

    private IEnumerator stopwatch() {
        while (time > 0) {
            yield return new WaitForSeconds(1f);
            time -= 1;
            UpdatedTimeUI(time);
        }
        TimeOvered?.Invoke();
    }
}
