using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyExpenditureControler : MonoBehaviour {
    private float spendForSecond = 1f;
    #region EVENTS
    public event Action<float> ExpecturedEnergy = delegate { };
    #endregion
    void Start() {
        StartCoroutine(spendEnergy());
    }
    private IEnumerator spendEnergy() {
        while (true) {
            yield return new WaitForSeconds(1f);
            ExpecturedEnergy(spendForSecond);
        }
    }
}
