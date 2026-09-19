using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindControler : MonoBehaviour {
    //0 a 11 km/h não produz energia
    //11 a 90 km/h é a faixa que produz
    //43 a 54 km/h produz com 100% de eficacia
    //acima de 90 km/h é cut-out
    public static WindControler windControler;
    public float windSpeed = 47;

    // Wind Varibles ===============================================
    public readonly float[] windProduce = { 0f, 11f, 43f, 54f, 90f };
    // 0 -> 1 Nao produz
    // 1 -> 2 Produz com uma porcentagem abaixo de 100%
    // 2 -> 3 Produz com 100%
    // 3 -> 4 Produz com uma porcentagem abaixo de 100% -> REMOVIDO
    // 4 -> Infinity Não produz cut-out
    //==============================================================
    void Awake() {
        if (windControler == null) windControler = this;
        else Destroy(this);
    }
}
