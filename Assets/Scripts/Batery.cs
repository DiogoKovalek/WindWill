using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batery : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprRenFull;
    private float sprHight;
    [SerializeField] private Sprite[] sprites;
    private readonly float[] percentageIntervals = {0.2f, 0.5f};

    private float numFull = 50;
    private const float maxFull = 100f;
    //Relacionado ao sprRenFull.size 
    private const float maxFullHight = 5;
    private const float minFullHight = 0.5f; 

    #region EVENTS
    public event Action GameOvered = delegate {}; 
    #endregion

    void Start() {
        sprHight = sprRenFull != null ? sprRenFull.size.y : 0;
    }

    void Update() {
        if(sprRenFull != null) {
            updateHightFull();
        }
    }

    private void updateHightFull() {
        float percentFull = Mathf.InverseLerp(0, maxFull, numFull);

        //Hight
        float hight = Mathf.Lerp(minFullHight, maxFullHight, percentFull);
        sprRenFull.size = new Vector2(sprRenFull.size.x, hight);

        //Color
        if(percentFull >= percentageIntervals[1]) {
            if(sprRenFull.sprite != sprites[0]){
                sprRenFull.sprite = sprites[0]; // para não ficar trocando toda hora
            }
        }else if(percentFull >= percentageIntervals[0]) {
            if(sprRenFull.sprite != sprites[1]) sprRenFull.sprite = sprites[1];
        }
        else{
            if(sprRenFull.sprite != sprites[2]) sprRenFull.sprite = sprites[2];
        }
    }

    public void OnReciveEnergy(float energy) {
        numFull = numFull + energy > 100 ? 100 : numFull + energy;
    }
}
