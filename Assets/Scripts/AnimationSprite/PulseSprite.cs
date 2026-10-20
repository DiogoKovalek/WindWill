using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseSprite : MonoBehaviour {
    [SerializeField] private float maxPulse = 1.3f;
    [SerializeField] private float minPulse = 0.8f;
    [SerializeField] private float timeToPulse = 1f;
    private float t = 0;
    private bool isGrow = true;

    void Start() {
        transform.localScale = Vector3.one * minPulse;
    }

    void Update() {
        t += Time.deltaTime;
        if (isGrow) transform.localScale = Vector3.one * Mathf.Lerp(minPulse, maxPulse, t/timeToPulse);
        else transform.localScale = Vector3.one * Mathf.Lerp(maxPulse, minPulse, t/timeToPulse);

        if(t >= timeToPulse) {
            t = 0;
            isGrow = !isGrow;
        }
    }
}
