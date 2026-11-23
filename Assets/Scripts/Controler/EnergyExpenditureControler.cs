using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyExpenditureControler : MonoBehaviour {
    private float percentSpendForSecond = 0.05f;
    #region EVENTS
    public event Action<float> ExpecturedEnergy = delegate { };
    public event Action<float> UpdatedTextSpendHouses = delegate {};
    #endregion
    void Start() {
        StartCoroutine(spendEnergy());
        //Valor vai ser fixo para cada fase
        UpdatedTextSpendHouses?.Invoke(percentSpendForSecond*100);
    }
    private IEnumerator spendEnergy() {
        while (true) {
            yield return new WaitForSeconds(1f);
            ExpecturedEnergy(percentSpendForSecond*100);
        }
    }
}
